using CoolWebsite.Application.Services.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class PdfReceiptVm
    {
        public CreateReceiptModel CreateReceiptModel { get; set; } = null!;
        public PdfReceiptDto PdfReceiptDto { get; set; } = null!;
    }
}