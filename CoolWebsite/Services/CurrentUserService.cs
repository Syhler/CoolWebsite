using System.Security.Claims;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserID = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            UserClaims = httpContextAccessor.HttpContext?.User;

            // var test = manager.GetUserAsync(httpContextAccessor.HttpContext?.User);

        }
        
        public string UserID { get; set; }
        
        public ClaimsPrincipal UserClaims { get; set; }
    }
}    