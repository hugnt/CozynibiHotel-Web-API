
using CozynibiHotel.API.Models;
using HUG.EmailServices;
using HUG.EmailServices.Models;
using HUG.EmailServices.Services;
using HUG.QRCodeServices.Models;
using HUG.QRCodeServices.Services;
using HUG.SMS.Models;
using HUG.SMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CozynibiHotel.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SMSController : Controller
    {
        private readonly TwilioSettings _twilioSettings;
        private readonly ISMSService _smsService;
        public SMSController(IOptions<TwilioSettings> options, ISMSService smsService)
        {
            _twilioSettings = options.Value;
            _smsService = smsService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult SendSMS([FromBody] SMS sms)
        {
            _smsService.Init(_twilioSettings);
            if (!_smsService.SendSMS(sms.PhoneNumber, sms.Content))
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
