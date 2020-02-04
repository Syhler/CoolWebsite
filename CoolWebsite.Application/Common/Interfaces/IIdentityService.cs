using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Models;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userID);

        Task<Result> CreateUserAsync(string email, string password);

        Task<Result> LoginUser(string email, string password);

        Task<Result> CreateRole(string roleName);
        
        void Logout();
        
        IQueryable<ApplicationRole> GetRoles();
    }
}