using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class FinancialProjectApplicationUser
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public string FinancialProjectId { get; set; }
        public FinancialProject FinancialProject { get; set; }

    }
}