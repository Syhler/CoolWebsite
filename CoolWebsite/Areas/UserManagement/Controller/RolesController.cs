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
        private readonly CurrentUserService _currentUserService;

        public RolesController(IHttpContextAccessor accessor, IIdentityService identityService)
        {
            _currentUserService = new CurrentUserService(accessor);
            _identityService = identityService;
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