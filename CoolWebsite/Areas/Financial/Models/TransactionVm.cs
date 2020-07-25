using System;
using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class TransactionVm
    {
        public TransactionPartialModel? TransactionPartialModel { get; set; }
        
        public List<FinancialProjectDto>? Projects { get; set; }

        public string? SelectedProject { get; set; }
    }
}