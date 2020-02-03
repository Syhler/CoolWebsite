using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoolWebsite.Areas.UserManagement.Controller
{
    
    [Area("UserManagement")]
    public class RolesController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IdentityService _identityService;
        private readonly CurrentUserService _currentUserService;

        public RolesController(ICurrentUserService service, IHttpContextAccessor accessor)
        {
            _currentUserService = new CurrentUserService(accessor);
            //_identityService = new IdentityService(userManager, roleManager);
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole()
        {
            return PartialView("_Layout");
        }
    }
}