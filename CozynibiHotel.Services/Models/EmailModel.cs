using HUG.EmailServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Models
{
    public class EmailModel
    {
        public EmailSettings EmailSettings { get; set; }
        public string ToAddress { get; set; }
        public string CustommerName { get; set; }
        public byte[] QRcode { get; set; }
        public int CheckInCode { get; set; }
        public string FilePath { get; set; }
    }
}
