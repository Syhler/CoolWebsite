using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Domain.Entities.Identity;

namespace BlazorServerWithAuth.Services.Common.Interface
{
    public interface IIdentityFacade
    {
        public List<ApplicationUser> GetAllUsers();

        public Task<ApplicationUser> GetUserById(string id);


    }
}