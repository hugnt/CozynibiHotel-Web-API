using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.CRUD.Services
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
        public ResponseModel(int status, string statusMessage, object data = null)
        {
            Status = status;
            StatusMessage = statusMessage;
            Data = data;
        }
    }
}
