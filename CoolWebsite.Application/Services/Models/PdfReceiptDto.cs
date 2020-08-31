using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolWebsite.Application.Services.Models
{
    public class PdfReceiptDto
    {
        public DateTime DateVisited { get; set; }
        public string Location { get; set; } = null!;
        public List<PdfReceiptItemDto> PdfReceiptItems { get; set; } = null!;

        public double Total => PdfReceiptItems.Sum(x => x.Count * x.Price);
           
    }
}