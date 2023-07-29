using HUG.SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace HUG.SMS.Services
{
    public class SMSService : ISMSService
    {
        private TwilioSettings _twilioSettings;
        public void Init(TwilioSettings twilioSettings)
        {
            _twilioSettings = twilioSettings;
        }
        public bool SendSMS(string phoneNumber, string content)
        {
            try
            {
                string accountSid = _twilioSettings.AccountSID;
                string authToken = _twilioSettings.AuthToken;
                string fromPhoneNumber = _twilioSettings.PhoneNumber;
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: content,
                    from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
                Console.WriteLine(message.Sid);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
           
        }
    }
}
