using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }

        public TokenModel()
        {

        }
        public TokenModel(string accessToken, string refeshToken)
        {
            AccessToken = accessToken;
            RefeshToken = refeshToken;
        }

    }
}
