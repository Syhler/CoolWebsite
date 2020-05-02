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
    public class UsersController : MediatorController
    {
        
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UsersController(IIdentityService identityService, ICurrentUserService currentUserService, IMapper mapper)
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
                CreateUpdateUserViewModel = new CreateUpdateUserViewModel
                {
                    Roles = CreateSelectedList(mappedRoles),
                }
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUpdateUserViewModel updateUserModel)
        {
            var user = new ApplicationUser
            {
                Email = updateUserModel.Email,
                FirstName = updateUserModel.FirstName,
                LastName = updateUserModel.LastName,
                UserName = updateUserModel.Email
            };

            var createdUser = _identityService.CreateUserAsync(user, updateUserModel.Password).Result;

            if (!createdUser.result.Succeeded) return View(updateUserModel);
            
            var addedRole = await _identityService.AddRoleToUser(createdUser.userId, updateUserModel.RoleName);
            if (addedRole.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View(updateUserModel);
        }

      
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _identityService.DeleteUser(id);

            return RedirectToAction("Index");
        }


        
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            ApplicationUser user = await _identityService.GetUserById(id);

            if (user == null) return PartialView("Error");

            
            var roleName = await _identityService.GetRolesByUser(user.Id);
            
            var mapped = _mapper.Map<CreateUpdateUserViewModel>(user);
            mapped.RoleName = roleName.FirstOrDefault();
            
            return PartialView(mapped);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateUserPost(CreateUpdateUserViewModel updateUserViewModel)
        {

            var user = _mapper.Map<ApplicationUser>(updateUserViewModel);
            
            var result = await _identityService.UpdateUser(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("UpdateUser", updateUserViewModel.Id);

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

    }
}