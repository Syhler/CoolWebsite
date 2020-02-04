using System.Security.Claims;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoolWebsite.Areas.UserManagement.Controller
{
    
    [Area("UserManagement")]
    public class RolesController : MediatorController
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public RolesController(IIdentityService identityService, ICurrentUserService currentUserService, IHttpContextAccessor accessor)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
            var userId = accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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

        [HttpPost]
        public async Task<IActionResult> AddEntity()
        {
            var command = new CreateTestEntityCommand
            {
                Name = "TestName"
            };
            await Mediator.Send(command);
            
            return RedirectToAction("Index");
        }
    }
}