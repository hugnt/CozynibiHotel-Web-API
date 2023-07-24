using AutoMapper;
using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Services.Interfaces;
using CozynibiHotel.Services.Models;
using HUG.CRUD.Services;
using HUG.EmailServices.Models;
using HUG.EmailServices.Services;
using HUG.QRCodeServices.Models;
using HUG.QRCodeServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CozynibiHotel.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IQRCodeService _qRCodeService;
        private const string SECRET_KEY = "HungNT1302Thebestdeveloperoftime";

        public BookingService(IBookingRepository bookingRepository,
                            IMapper mapper,
                            IEmailService emailService,
                            IQRCodeService qRCodeService)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _emailService = emailService;
            _qRCodeService = qRCodeService;
        }

        public BookingDto GetBooking(int bookingId)
        {
            if (!_bookingRepository.IsExists(bookingId)) return null;
            var bookingMap = _mapper.Map<BookingDto>(_bookingRepository.GetById(bookingId));
            return bookingMap;
        }

        public IEnumerable<BookingDto> GetBookings()
        {
            var bookingsMap = _mapper.Map<List<BookingDto>>(_bookingRepository.GetAll());
            return bookingsMap;
        }
        public ResponseModel CreateBooking(BookingDto bookingCreate)
        {
            if (bookingCreate.CreatedBy == 0) bookingCreate.CreatedBy = 1;
            if (bookingCreate.UpdatedBy == 0) bookingCreate.UpdatedBy = 1;
            bookingCreate.CreatedAt = DateTime.Now;
            bookingCreate.IsActive = true;
            bookingCreate.IsDeleted = false;
            var bookings = _bookingRepository.GetAll()
                            .Where(l => l.FullName.Trim().ToLower() == bookingCreate.FullName.Trim().ToLower()
                                        && l.Email.Trim().ToLower() == bookingCreate.Email.Trim().ToLower()
                                        && l.PhoneNumber == bookingCreate.PhoneNumber)
                            .FirstOrDefault();
            if (bookings != null)
            {
                return new ResponseModel(422, "Booking already exists");
            }


            var bookingMap = _mapper.Map<Booking>(bookingCreate);
            bookingMap.CreatedAt = DateTime.Now;


            if (!_bookingRepository.Create(bookingMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateBooking(int bookingId, BookingDto updatedBooking)
        {
            if (updatedBooking.CreatedBy == 0) updatedBooking.CreatedBy = 1;
            if (updatedBooking.UpdatedBy == 0) updatedBooking.UpdatedBy = 1;
            updatedBooking.UpdatedAt = DateTime.Now;

            if (!_bookingRepository.IsExists(bookingId)) return new ResponseModel(404, "Not found");
            var bookingMap = _mapper.Map<Booking>(updatedBooking);
            if (!_bookingRepository.Update(bookingMap))
            {
                return new ResponseModel(500, "Something went wrong updating booking");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteBooking(int bookingId)
        {
            if (!_bookingRepository.IsExists(bookingId)) return new ResponseModel(404, "Not found");
            var bookingToDelete = _bookingRepository.GetById(bookingId);
            if (!_bookingRepository.Delete(bookingToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting booking");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<BookingDto> SearchBookings(string field, string keyWords)
        {
            var res = _bookingRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateBooking(int bookingId, bool isDelete)
        {
            if (!_bookingRepository.SetDelete(bookingId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete booking");
            }
            return new ResponseModel(204, "");
        }
        public ResponseModel UpdateBookingStatus(int bookingId, bool status)
        {
            if (!_bookingRepository.SetStatus(bookingId, status))
            {
                return new ResponseModel(500, "Something went wrong when updaing status booking");
            }
            return new ResponseModel(204, "");
        }

        public async Task<ResponseModel> ConfirmBooking(int bookingId, EmailSettings emailSettings)
        {
            if (!_bookingRepository.IsExists(bookingId)) return new ResponseModel(404, "");
            var bookingMap = _mapper.Map<BookingDto>(_bookingRepository.GetById(bookingId));
            //Create checkin code
            var checkInCode = CreateCheckInCode();
            //create Qr
            var qrCode = CreateQRCode(bookingMap);
            //Send email
            var emailModel = new EmailModel();
            emailModel.EmailSettings = emailSettings;
            emailModel.ToAddress = bookingMap.Email;
            emailModel.CustommerName = bookingMap.FullName;
            emailModel.QRcode = qrCode;
            emailModel.CheckInCode = checkInCode;
            bool isSendMailSuccess = await SendMail(emailModel);
            if (isSendMailSuccess) {
                //Saving checkin code in db
                if (!_bookingRepository.UpdateCheckInCode(bookingId, checkInCode))
                {
                    return new ResponseModel(500, "Something went wrong updating CheckInCode");
                }
                return new ResponseModel(201, "");
            }
            return new ResponseModel(500, "Send mail fail");
        }

        public BookingDto ValidateQRBooking(string qrToken)
        {
            int validQR = ValidateToken(qrToken);
            if(validQR == -1) return null;
            if (!_bookingRepository.IsExists(validQR)) return null;
            return _mapper.Map<BookingDto>(_bookingRepository.GetById(validQR));
        }

        private async Task<bool> SendMail(EmailModel email)
        {
            var htmlEmail =
                "<div style=\"width: 100%;text-align: center;background-color: #d7b659;\">" +
                    "<div style=\"background-color:#fff;width: 100%\">" +
                        "<div style=\"width: 200px; height: 200px;margin: auto;\">" +
                            "<img src=\"" + "https://cozynibi.com/Uploads/images/ads/logo.png" + "\" alt=\"qrcode\" style=\"width: 100%;height: 100%;object-fit: contain;\">" +
                        " </div>" +
                    "</div>" +
                    "<div style=\"padding: 30px;\">" +
                        "<h1 style=\"text-transform: uppercase;\">Welcome to Cozinibi Hotel</h1>" +
                        "<h2>Hello, " + email.CustommerName + "</h2> " +
                        "<h3 style=\"color: red;\">🥳🎉 Thank you very much for your booking 👏🤝</h3>" +
                        "<span>Here is your <b>Checkin code</b></span> " +
                        "<div style=\"width: 400px; height: 400px;margin: auto;\">" +
                            " <img src=\"cid:{0}\" alt=\"qrcode\" style=\"width: 100%;height: 100%;object-fit: contain;\"> " +
                        "</div>" +
                        " <h3>Checkin code: " + email.CheckInCode + "</h3> " +
                        "<p>Please keep and show this code for the receptions to have the instruction ❤️‍</p>" +
                        "<div style=\"text-align: left;\">" +
                            " <h4>If you have any questions, contact us by</h4>" +
                            "<i><b>Email: </b></i><span>thanh.hung.st302@gmail.com</span> <br/>" +
                            " <i><b>Phone number: </b></i><span>0946928815</span>" +
                        " </div>" +
                    "</div>" +
                "</div>";

            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = email.ToAddress,
                Subject = "Cozinibi Hotel - Booking successfully",
                Body = htmlEmail,
                ImageSourceByte = email.QRcode
            };
            try
            {
                await _emailService.SendEmailAsync(email.EmailSettings, mailRequest);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        private byte[] CreateQRCode(BookingDto bookingDto)
        {
            
            var qrCodeModel = new QRCodeModel();
            qrCodeModel.QRCodeText = GenerateToken(bookingDto);
            var imgByte = _qRCodeService.CreateQRCode(qrCodeModel);
            return imgByte;
        }

        private int CreateCheckInCode()
        {
            int checkInCode = 0;
            do
            {
                Random random = new Random();
                checkInCode = random.Next(100000, 999999);
            }
            while (_bookingRepository.GetAll().Any(x => x.CheckInCode== checkInCode));

            return checkInCode;
        }
        
        private string GenerateToken(BookingDto booking)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(SECRET_KEY);
            int timeExpire = -1;
            if (booking.CheckIn!=null && booking.CheckIn >= DateTime.Now)
            {
                TimeSpan duration = (booking.CheckIn - DateTime.Now) ?? TimeSpan.Zero;
                timeExpire = duration.Hours + 1;
            }
            
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name , booking.FullName),
                    new Claim("id" , booking.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
                }),

                Expires = DateTime.UtcNow.AddHours(timeExpire),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var qrToken = jwtTokenHandler.WriteToken(token);

            return qrToken;
        }

        private int ValidateToken(string qrToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(SECRET_KEY);
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
                var tokenInVerification = jwtTokenHandler.ValidateToken(qrToken,
                    tokenValidateParam, out var validatedToken);

                //check 2: check algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg
                                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return -1;
                    }
                }

                var ID = tokenInVerification.FindFirst("id")?.Value;
                if (!string.IsNullOrEmpty(ID))
                {
                    return int.Parse(ID);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {

                return - 1;
            }

            return -1;
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;

        }

    }
}
