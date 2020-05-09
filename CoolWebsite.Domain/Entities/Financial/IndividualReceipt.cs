using CoolWebsite.Domain.Common;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class IndividualReceipt : AudibleEntity
    {
        public string Id { get; set; }
        public double Total { get; set; }
        public ApplicationUser User { get; set; }
        
        public Receipt Receipt { get; set; }
    }
}