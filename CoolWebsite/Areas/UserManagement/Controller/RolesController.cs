using System.Security.Claims;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.UpdateTestEntity;
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

        public RolesController(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
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
                Name = "TestName910"
            };
            await Mediator.Send(command);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEntity()
        {
            var command = new UpdateTestEntityCommand
            {
                Name = "new name",
                Id = "d3a6d43e-e473-4b4e-a9df-d910e254597e"
            };
            await Mediator.Send(command);

            return RedirectToAction("Index");
        }
    }
}