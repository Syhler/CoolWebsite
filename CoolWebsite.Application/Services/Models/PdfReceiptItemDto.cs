using System;

namespace CoolWebsite.Application.Services.Models
{
    public class PdfReceiptItemDto
    {
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public double Price { get; set; }
        public bool IsDiscount { get; set; }

        public double Total => Price * Count;
    }
}