using System;
using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class CreateReceiptModel
    {
        //public string Location { get; set; }
        
        //public string Note { get; set; }
        
        //public DateTime DateVisited { get; set; }

        public string FinancialProjectId { get; set; }

        //public List<ReceiptItemDto> ReceiptItemModels { get; set; }

        public ReceiptDto ReceiptDto { get; set; }
        public CreateReceiptItemVm CreateReceiptItemVm { get; set; }
    }
}