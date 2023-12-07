using GenerateBarcodeAndQRcodeMVCCore5.Models;
using Microsoft.AspNetCore.Mvc;
using static QRCoder.PayloadGenerator;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using QRCoder;
using System;

namespace GenerateBarcodeAndQRcodeMVCCore5.Controllers
{
    public class QRcodeController : Controller
    {
        public IActionResult Index()
        {
            GenerateQRCodeModel model = new GenerateQRCodeModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(GenerateQRCodeModel model)
        {

            Payload payload = null;

            switch (model.QRCodeType)
            {
                case 1: // website url
                    payload = new Url(model.WebsiteURL);
                    break;
                case 2: // bookmark url
                    payload = new Bookmark(model.BookmarkURL, model.BookmarkURL);
                    break;
                case 3: // compose sms
                    payload = new SMS(model.SMSPhoneNumber, model.SMSBody);
                    break;
                case 4: // compose whatsapp message
                    payload = new WhatsAppMessage(model.WhatsAppNumber, model.WhatsAppMessage);
                    break;
                case 5://compose email
                    payload = new Mail(model.ReceiverEmailAddress, model.EmailSubject, model.EmailMessage);
                    break;
                case 6: // wifi qr code
                    payload = new WiFi(model.WIFIName, model.WIFIPassword, WiFi.Authentication.WPA);
                    break;
                case 7: // wifi qr code
                    payload = new SMS(model.SMSBody);
                    break;

            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);

            //// use this when you want to show your logo in middle of QR Code and change color of qr code
            ////Bitmap logoImage = new Bitmap(@"wwwroot/img/Virat-Kohli.jpg");
            ////var qrCodeAsBitmap = qrCode.GetGraphic(20, Color.Black, Color.White, logoImage);

            string base64String = Convert.ToBase64String(BitmapToByteArray(qrCodeAsBitmap));
            model.QRImageURL = "data:image/png;base64," + base64String;
            return View("Index", model);
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
