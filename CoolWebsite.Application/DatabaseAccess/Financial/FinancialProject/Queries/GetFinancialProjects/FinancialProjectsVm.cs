using System.Collections.Generic;
using CoolWebsite.Application.Common.Mapping;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects
{
    public class FinancialProjectsVm
    {
        public IList<FinancialProjectDto> FinancialProjects { get; set; }
    }
}