
using CozynibiHotel.API.Models;
using HUG.EmailServices;
using HUG.EmailServices.Models;
using HUG.EmailServices.Services;
using HUG.QRCodeServices.Models;
using HUG.QRCodeServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CozynibiHotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        //private readonly IEmailService _emailService;
        //private readonly EmailSettings _emailSettings;
        //private readonly IWebHostEnvironment _environment;
        //private readonly IQRCodeService _qRCodeService;
        //public MailController(IEmailService emailService, IOptions<EmailSettings> options, IWebHostEnvironment environment, IQRCodeService qRCodeService)
        //{
        //    _emailService = emailService;
        //    _environment = environment;
        //    _qRCodeService = qRCodeService;
        //    _emailSettings = options.Value;
        //}

        //[HttpPost("SendEmail")]
        //public async Task<IActionResult> SendMail(Email email)
        //{
        //    var qrCodeModel = new QRCodeModel();
        //    qrCodeModel.QRCodeText = "Nguyen Thanh Hung";
        //    var imgByte = _qRCodeService.CreateQRCode(qrCodeModel);
        //    var filePath = Path.Combine(_environment.WebRootPath, "images","message.png");
        //    var htmlEmail = 
        //        "<div style=\"width: 100%;text-align: center;background-color: #d7b659;\">" +
        //            "<div style=\"background-color:#fff;width: 100%\">" +
        //                "<div style=\"width: 200px; height: 200px;margin: auto;\">" +
        //                    "<img src=\""+ "https://cozynibi.com/Uploads/images/ads/logo.png" +"\" alt=\"qrcode\" style=\"width: 100%;height: 100%;object-fit: contain;\">" +
        //                " </div>" +
        //            "</div>" +
        //            "<div style=\"padding: 30px;\">" +
        //                "<h1 style=\"text-transform: uppercase;\">Welcome to Cozinibi Hotel</h1>" +
        //                "<h2>Hello, "+email.CustommerName+"</h2> " +
        //                "<h3 style=\"color: red;\">🥳🎉 Thank you very much for your booking 👏🤝</h3>" +
        //                "<span>Here is your <b>Checkin code</b></span> " +
        //                "<div style=\"width: 400px; height: 400px;margin: auto;\">" +
        //                    " <img src=\"cid:{0}\" alt=\"qrcode\" style=\"width: 100%;height: 100%;object-fit: contain;\"> " +
        //                "</div>" +
        //                " <h3>Checkin code: " + email.CheckinCode + "</h3> " +
        //                "<p>Please keep and show this code for the receptions to have the instruction ❤️‍</p>" +
        //                "<div style=\"text-align: left;\">" +
        //                    " <h4>If you have any questions, contact us by</h4>" +
        //                    "<i><b>Email: </b></i><span>thanh.hung.st302@gmail.com</span> <br/>" +
        //                    " <i><b>Phone number: </b></i><span>0946928815</span>" +
        //                " </div>" +
        //            "</div>"+
        //        "</div>";

        //    MailRequest mailRequest = new MailRequest()
        //    {
        //        ToEmail = email.ToAddress,
        //        Subject = "Cozinibi Hotel - Booking successfully",
        //        Body = htmlEmail,
        //        ImageSourceByte = imgByte,
        //        FileName = "message.png",
        //        FileSource = filePath
        //    };
        //    try
        //    {
        //        await _emailService.SendEmailAsync(_emailSettings, mailRequest);
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
            
        //}
      
    }
}
