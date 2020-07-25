using System;
using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class TransactionVm
    {
        public List<object> PayedTransaction { get; set; }
        
        public List<Object> RecivedTransaction { get; set; }
        
        public List<FinancialProjectDto> Projects { get; set; }
    }
}