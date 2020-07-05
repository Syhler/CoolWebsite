namespace CoolWebsite.Areas.Financial.Models
{
    public class PayTransaction
    {
        public string ToUserId { get; set; }
        public double Amount { get; set; }
        public string FinancialProjectId { get; set; }
        
    }
}