using System.Collections.Generic;
using CoolWebsite.Domain.Entities.Financial;
using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public ICollection<FinancialProjectApplicationUser> FinancialProjectApplicationUsers { get; set; }

    }
}