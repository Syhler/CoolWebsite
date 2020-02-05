using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.UpdateTestEntity;
using CoolWebsite.Areas.UserManagement.Models;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Services;
using CoolWebsite.Services.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoolWebsite.Areas.UserManagement.Controller
{
    
    [Area("UserManagement")]
    [Authorize(Roles = "Admin")]
    public class RolesController : MediatorController
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public RolesController(IIdentityService identityService, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _identityService = identityService;
        }
        
        // GET
        public IActionResult Index() 
        {
        
            var test = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<ApplicationRole, RoleViewModel>();
                cfg.AddProfile(new VMMappingProfile());
            });

            
            var roles = _identityService.GetRoles();

            var result = roles.ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider).ToList();
            
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole()
        {
            return PartialView("_Layout");
        }

        [HttpPost]
        public async Task<IActionResult> AddEntity()
        {
            await _identityService.CreateRole("TestRole");
            
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