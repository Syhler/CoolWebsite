using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Areas.UserManagement.Models;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.UserManagement.Controller
{
    
    [Area("UserManagement")]
    [Authorize(Roles = "Admin")]
    public class UserController : MediatorController
    {
        
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserController(IIdentityService identityService, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _identityService = identityService;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var users = _identityService.GetUsers();
            var roles = _identityService.GetRoles();
            var mappedUsers = users.ProjectTo<UserModel>(_mapper.ConfigurationProvider).ToList();
            var mappedRoles = roles.ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider).ToList();

            foreach (var userModel in mappedUsers)
            {
                userModel.RoleNames = (List<string>) await _identityService.GetRolesByUser(userModel.Id);
            }
            
            var viewModel = new UsersViewModel
            {
                Users = mappedUsers,
                CreateUserViewModel = new CreateUserViewModel
                {
                    Roles = CreateSelectedList(mappedRoles),
                }
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel userModel)
        {
            var user = new ApplicationUser
            {
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                UserName = userModel.Email
            };

            var createdUser = _identityService.CreateUserAsync(user, userModel.Password).Result;

            if (!createdUser.result.Succeeded) return View(userModel);
            
            var addedRole = await _identityService.AddRoleToUser(createdUser.userId, userModel.RoleName);
            if (addedRole.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View(userModel);
        }

        private List<SelectListItem> CreateSelectedList(List<RoleViewModel> models)
        {
            var listOfSelectedItem = new List<SelectListItem>();

            foreach (var roleViewModel in models)
            {
                listOfSelectedItem.Add(new SelectListItem
                {
                    Value = roleViewModel.Name,
                    Text = roleViewModel.Name
                });
            }

            return listOfSelectedItem;
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _identityService.DeleteUser(id);

            return RedirectToAction("Index");
        }
    }
}