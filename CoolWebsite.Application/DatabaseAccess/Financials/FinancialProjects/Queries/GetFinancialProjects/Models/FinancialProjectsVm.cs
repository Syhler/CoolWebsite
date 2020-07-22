using System.Collections.Generic;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class FinancialProjectsVm
    {
        public IList<FinancialProjectDto>? FinancialProjects { get; set; } 
    }
}