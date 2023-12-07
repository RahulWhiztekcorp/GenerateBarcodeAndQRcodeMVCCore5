using GenerateBarcodeAndQRcodeMVCCore5.Models;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace GenerateBarcodeAndQRcodeMVCCore5.Controllers
{
    public class BarcodeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public BarcodeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(GenerateBarcodeModel generateBarcode)
        {
            try
            {
                GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(generateBarcode.BarcodeText, BarcodeWriterEncoding.Code128);
                barcode.ResizeTo(400, 120);
                barcode.AddBarcodeValueTextBelowBarcode();
                // Styling a Barcode and adding annotation text
                barcode.ChangeBarCodeColor(System.Drawing.Color.Black);
                barcode.SetMargins(10);
                string path = Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                barcode.SaveAsPng(filePath);
                string fileName = Path.GetFileName(filePath);
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                ViewBag.QrCodeUri = imageUrl;
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
    }
}
