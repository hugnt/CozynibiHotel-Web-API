using HUG.QRCodeServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.QRCodeServices.Services
{
    public interface IQRCodeService
    {
        byte[] CreateQRCode(QRCodeModel qRCode);
    }
}
