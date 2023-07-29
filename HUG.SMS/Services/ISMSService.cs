using HUG.SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.SMS.Services
{
    public interface ISMSService
    {
        void Init(TwilioSettings twilioSettings);
        bool SendSMS(string phoneNumber, string content);
    }
}
