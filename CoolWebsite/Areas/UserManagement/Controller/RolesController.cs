using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Areas.UserManagement.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index() 
        {
            
            var roles = _identityService.GetRoles();
            

            var roleViewModels = roles.ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider).ToList();

            foreach (var role in roleViewModels)
            {
                var users = await _identityService.GetUsersByRole(role.Name);

                role.Users = users.ProjectTo<UserModel>(_mapper.ConfigurationProvider).ToList();

            }
            
            
            return View(roleViewModels);
        }

        
        public IActionResult DeleteRole(string id)
        {
            Console.WriteLine("delete role : " + id);
            
            var result = _identityService.DeleteRole(id).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Privacy", "Home", new {area=""});

        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRole()
        {
            return PartialView("_Layout");
        }

        [HttpPost]
        public async Task<IActionResult> AddEntity()
        {
            await _identityService.CreateRole("asdasd");
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEntity()
        {
            

            return RedirectToAction("Index");
        }

     
    }
}