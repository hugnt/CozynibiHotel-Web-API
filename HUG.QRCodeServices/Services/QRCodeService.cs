using HUG.QRCodeServices.Models;
using QRCoder;
using System.Drawing.Imaging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HUG.QRCodeServices.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] CreateQRCode(QRCodeModel qRCode)
        {
            try
            {
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.QRCodeText, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(60);
                byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                //string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                return BitmapArray;
            }
            catch (Exception)
            {
                return null;
            }
            return null;   
         
        }

    }

    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
