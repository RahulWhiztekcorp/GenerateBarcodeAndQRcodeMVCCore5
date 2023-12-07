using System.ComponentModel.DataAnnotations;

namespace GenerateBarcodeAndQRcodeMVCCore5.Models
{
    public class GenerateBarcodeModel
    {
        [Display(Name = "Enter Barcode Text")]
        public string BarcodeText { get; set; }
    }
}
