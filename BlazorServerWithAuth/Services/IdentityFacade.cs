using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWithAuth.Services.Common.Interface;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Identity;

namespace BlazorServerWithAuth.Services
{
    public class IdentityFacade : IIdentityFacade
    {
        private readonly IIdentityService _identityService;

        public IdentityFacade(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _identityService.GetUsers().ToList();
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _identityService.GetUserById(id);

        }
        
    }
}