using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Services
{
    public static class MobilePayDeepLink
    {
        
        //link: https://www.mobilepay.dk/erhverv/betalingslink/betalingslink-svar?phone=41131865&amount=100.30&comment=n%C3%A6
        
        public static string GenerateUrl(ApplicationUser user, double amount)
        {
            var comment = "CoolWebsite";
            return $"https://www.mobilepay.dk/erhverv/betalingslink/betalingslink-svar?phone={user.PhoneNumber}&amount={amount}&comment={comment}";
        }
        
    }
}