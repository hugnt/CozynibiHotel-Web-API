using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.EmailServices.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileSource { get; set; }
        public string FileName { get; set; }
        public byte[] ImageSourceByte { get; set; }
    }
}
