using System.Security.Claims;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; set; } 
    }
}