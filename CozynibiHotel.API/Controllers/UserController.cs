using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using CozynibiHotel.Services.Models;
using Microsoft.AspNetCore.Authorization;

namespace CozynibiHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;
        

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        
        [HttpGet("VerifyToken")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            return Ok("Valid Token");
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(AccountDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Validate(AccountDto account)
        {
            var user = await _accountService.ValidateAccount(account);
            if (user.Status != 200)
            {
                ModelState.AddModelError("", user.StatusMessage);
                return StatusCode(user.Status, ModelState);
            }

            //Cấp token
            return Ok(user);
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var renewToken = await _accountService.RenewToken(model);
            if (renewToken.Status != 200)
            {
                ModelState.AddModelError("", renewToken.StatusMessage);
                return StatusCode(renewToken.Status, ModelState);
            }

            //Cấp token
            return Ok(renewToken);
        }
        


    }
}
