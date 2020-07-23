using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Models;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userID);

        Task<ApplicationUser> GetUserByEmail(string email);

        Task<(Result result, string userId)> CreateUserAsync(ApplicationUser user, string password);

        Task<Result> LoginUser(string email, string password, bool persistent);

        Task<Result> CreateRole(string roleName);

        Task<Result> DeleteRole(string id);

        Task<Result> AddRoleToUser(string name);

        Task<Result> AddRoleToUser(string userID, string name);

        Task<IList<string>> GetRolesByUser(string userID);
            
        void Logout();
        
        IQueryable<ApplicationRole> GetRoles();
        Task<IQueryable<ApplicationUser>> GetUsersByRole(string name);
        IQueryable<ApplicationUser> GetUsers();
        Task<Result> DeleteUser(string id);
        Task<ApplicationUser> GetUserById(string id);
        Task<IList<ApplicationUser>> GetUsersByIds(IList<string> ids);
        Task<Result> UpdateUser(UpdateApplicationUser updateApplicationUser);
    }
}