namespace CoolWebsite.Areas.Financial.Models
{
    public class PayTransaction
    {
        public string ToUserId { get; set; } = null!;
        public double Amount { get; set; }
        public string FinancialProjectId { get; set; } = null!;

    }
}