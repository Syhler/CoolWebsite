namespace CoolWebsite.Domain.Common.ValueObjects
{
    public class Payment
    {
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public decimal Amount { get; set; }
        public string ProjectId { get; set; }
        
    }
}