using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class ArchiveVm
    {
        public List<FinancialProjectDto>? FinancialProjects { get; set; }
        
        public List<ReceiptDto>? Receipts { get; set; }
    }
}