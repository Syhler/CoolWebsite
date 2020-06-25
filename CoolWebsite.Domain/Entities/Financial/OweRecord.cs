using System.Collections.Generic;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class OweRecord
    {
        public string Id { get; set; }

        public string FinancialProjectId { get; set; }
        public FinancialProject FinancialProject { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string OwedUserId { get; set; }
        public ApplicationUser OwedUser { get; set; }

        public double Amount { get; set; }

    }

   
}