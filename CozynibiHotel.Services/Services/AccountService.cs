using AutoMapper;
using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Helper;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Services.Interfaces;
using CozynibiHotel.Services.Models;
using HUG.CRUD.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefeshTokenRepository _refeshTokenRepository;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSettings;

        public AccountService(IAccountRepository accountRepository, IMapper mapper
            ,IOptionsMonitor<AppSetting> optionsMonitor
            , IRefeshTokenRepository refeshTokenRepository
            , IRoleRepository roleRepository
            )
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _appSettings = optionsMonitor.CurrentValue;
            _refeshTokenRepository = refeshTokenRepository;
            _roleRepository = roleRepository;
        }

        public Role GetRoleById(int roleId)
        {
            if (!_roleRepository.IsExists(roleId)) return null;
            return _roleRepository.GetById(roleId);
        }
        public AccountDto GetAccount(int accountId)
        {
            if (!_accountRepository.IsExists(accountId)) return null;
            var account = _mapper.Map<AccountDto>(_accountRepository.GetById(accountId));
            return account;
        }

        public IEnumerable<AccountDto> GetAccounts()
        {
            var accounts = _mapper.Map<List<AccountDto>>(_accountRepository.GetAll());
            return accounts;
        }
        public ResponseModel CreateAccount(AccountDto accountCreate)
        {
            var accounts = _accountRepository.GetAll()
                            .Where(l => l.Username.Trim().ToLower() == accountCreate.Username.Trim().ToLower())
                            .FirstOrDefault();
            if (accounts != null)
            {
                return new ResponseModel(422, "Account already exists");
            }

            var accountMap = _mapper.Map<Account>(accountCreate);

            if (!_accountRepository.Create(accountMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateAccount(int accountId, AccountDto updatedAccount)
        {
            if (!_accountRepository.IsExists(accountId)) return new ResponseModel(404, "Not found");
            var accountMap = _mapper.Map<Account>(updatedAccount);
            if (!_accountRepository.Update(accountMap))
            {
                return new ResponseModel(500, "Something went wrong updating account");
            }
            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteAccount(int accountId)
        {
            if (!_accountRepository.IsExists(accountId)) return new ResponseModel(404, "Not found");
            var accountToDelete = _accountRepository.GetById(accountId);
            if (!_accountRepository.Delete(accountToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting account");
            }
            return new ResponseModel(204, "");
        }

        public async Task<ResponseModel> ValidateAccount(AccountDto account)
        {
            if (!_accountRepository.IsExists(account))
            {
                return new ResponseModel(404, "Account is not exist");
            }

            var token = await GenerateToken(account);
            return new ResponseModel(
                200,
                "Login successfully",
                token
            );
        }

        private async Task<TokenModel> GenerateToken(AccountDto account)
        {
            var accountId = _accountRepository.GetAccount(account).Id;
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);



            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name , account.FullName),
                    new Claim(JwtRegisteredClaimNames.Email , account.Email),
                    new Claim(JwtRegisteredClaimNames.Sub , account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim("Username" , account.Username),
                    new Claim("Id" , accountId.ToString()),

                    //roles
                }),

                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accsessToken = jwtTokenHandler.WriteToken(token);
            var refeshToken = GenerateRefeshToken();

            //Save in db
            var refeshTokenCreate = new RefeshToken
            {
                JwtId = token.Id,
                AccountId = accountId,
                Token = refeshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddHours(1)
            };
            await _refeshTokenRepository.Create_Async(refeshTokenCreate);

            return new TokenModel
            {
                AccessToken = accsessToken,
                RefeshToken = refeshToken
            };
        }
        private string GenerateRefeshToken()
        {
            var random = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<ResponseModel> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko ktra token het han

            };

            try
            {
                //Check 1: accessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken,
                    tokenValidateParam, out var validatedToken);

                //check 2: check algorithm
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg
                                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if(!result)
                    {
                        return new ResponseModel(500, "Invalid token");
                    }
                }

                //check 3: check accessToken expire?
                var utcExpireDate =
                    long.Parse(tokenInVerification.Claims.FirstOrDefault(x =>
                        x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if(expireDate > DateTime.UtcNow)
                {
                    return new ResponseModel(200, "Access token has not yet expired",model);
                }

                //check 4: Check refeshtoken exists in db
                if (!_refeshTokenRepository.IsExists(model.RefeshToken))
                {
                    return new ResponseModel(500, "Refesh token does not exist");
                }

                //check 5: Check refeshtoken is used/revoked?
                if (!_refeshTokenRepository.IsValid(model.RefeshToken))
                {
                    return new ResponseModel(500, "Refesh token has been used or revoked");
                }

                //check 6: Access Token Id == jwtId in RefeshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => 
                    x.Type == JwtRegisteredClaimNames.Jti).Value;
                
                if(_refeshTokenRepository.GetRefeshToken(model.RefeshToken).JwtId != jti)
                {
                    return new ResponseModel(500, "Token does not match");
                }

                //Update token 
                var currentRefeshToken = _refeshTokenRepository.GetRefeshToken(model.RefeshToken);
                currentRefeshToken.IsRevoked = true;
                currentRefeshToken.IsUsed = true;
                _refeshTokenRepository.Update(currentRefeshToken);

                //Create new token
                var account = await _accountRepository.GetById_Async(currentRefeshToken.AccountId);
                var accountMap = _mapper.Map<AccountDto>(account);
                var token = await GenerateToken(accountMap);
                return new ResponseModel(
                    200,
                    "Renew Token successfully",
                    token
                );

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseModel(500, "Something went wrong");
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;

        }


    }


}
