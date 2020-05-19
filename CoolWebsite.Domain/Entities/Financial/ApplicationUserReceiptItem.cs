using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class ApplicationUserReceiptItem
    {
        public string ReceiptItemId { get; set; }
        
        public ReceiptItem ReceiptItem { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}