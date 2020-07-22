using System;
using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class ReceiptItemVm
    {
        public ReceiptItemDto ReceiptItem { get; set; } = null!;

        public Guid UniqueIdentifier { get; set; }
        
    }
}