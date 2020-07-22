using System;
using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class CreateReceiptModel
    {
        public string FinancialProjectId { get; set; } = null!;

        public ReceiptDto ReceiptDto { get; set; } = null!;
        public CreateReceiptItemVm? CreateReceiptItemVm { get; set; }
    }
}