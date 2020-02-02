using System.Collections.Generic;
using System.Linq;
using CoolWebsite.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
        }

        public static Result ToApplicationResult(this SignInResult result)
        {
            return result.Succeeded ? Result.Success() : Result.Failure(ToApplicationError(result));
        }

        private static List<string> ToApplicationError(SignInResult result)
        {
            var errors = new List<string>();
            
            if (result.IsLockedOut)
            {
                errors.Add("Account is locked out");
            }

            if (result.IsNotAllowed)
            {
                errors.Add("Account is not allowed to login");
            }

            if (result.RequiresTwoFactor)
            {
                errors.Add("Account requiresTwoFactor");
            }

            return errors;
        }
    }
}