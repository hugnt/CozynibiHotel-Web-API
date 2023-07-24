using HUG.EmailServices.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.EmailServices.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailSettings emailSettings,MailRequest mailRequest)
        {
            var email = new MimeMessage();
            MailboxAddress sender = new MailboxAddress(emailSettings.DisplayName, emailSettings.Email);
            email.Sender = sender;
            email.From.Add(sender);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            // Thêm hình ảnh vào body của email
            
            byte[] fileBytes;
            if (System.IO.File.Exists(mailRequest.FileSource))
            {
                FileStream file = new FileStream(mailRequest.FileSource, FileMode.Open, FileAccess.Read);
                using(var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                builder.Attachments.Add(mailRequest.FileName, fileBytes, ContentType.Parse("application/octet-stream"));
            }
            if (mailRequest.ImageSourceByte != null)
            {
                var image = builder.LinkedResources.Add("img.png",mailRequest.ImageSourceByte);
                image.ContentId = MimeUtils.GenerateMessageId();
                builder.HtmlBody = string.Format(mailRequest.Body, image.ContentId);
            }

            
            email.Body = builder.ToMessageBody();


            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
