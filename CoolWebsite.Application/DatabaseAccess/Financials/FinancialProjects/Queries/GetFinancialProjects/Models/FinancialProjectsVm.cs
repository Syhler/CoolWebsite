using System.Collections.Generic;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class FinancialProjectsVm
    {
        public FinancialProjectsVm()
        {
            FinancialProjects = new List<FinancialProjectDto>();
        }
        
        public IList<FinancialProjectDto> FinancialProjects { get; set; }

    }
}