using CoolWebsite.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            
        }

        public static Result ToApplicationResult(this SignInResult result)
        {
            
        }
    }
}