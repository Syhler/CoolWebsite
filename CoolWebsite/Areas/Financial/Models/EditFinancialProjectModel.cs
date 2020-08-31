using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class EditFinancialProjectModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public List<string> Users { get; set; } = null!;

        public AddUserModel AddUserModel { get; set; } = null!;


    }
}