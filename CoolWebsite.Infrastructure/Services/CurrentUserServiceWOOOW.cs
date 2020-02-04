using System.Security.Claims;
using CoolWebsite.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CoolWebsite.Infrastructure.Services
{
    public class CurrentUserServiceWOOOW : ICurrentUserService
    {
        public CurrentUserServiceWOOOW(IHttpContextAccessor httpContextAccessor)
        {
            UserID = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        }
        
    
        public string UserID { get; set; }
    }
}