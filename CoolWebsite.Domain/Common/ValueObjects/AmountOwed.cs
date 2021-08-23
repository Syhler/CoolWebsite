namespace CoolWebsite.Domain.Common.ValueObjects
{
    public class AmountOwed
    {
        public string ReceiptId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string ProjectId { get; set; }
    }
}