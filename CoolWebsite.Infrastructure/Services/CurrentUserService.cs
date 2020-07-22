using System.Security.Claims;
using CoolWebsite.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CoolWebsite.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserClaims = httpContextAccessor.HttpContext.User;
        }
        
    
        public string UserId { get; set; }
        public ClaimsPrincipal? UserClaims { get; set; }
    }
}