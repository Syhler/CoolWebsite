using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.Common.Models;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;

        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
        }
        public async Task<string> GetUserNameAsync(string userID)
        {
            throw new System.NotImplementedException();
        }

        public async Task<(Result result, string userId)> CreateUserAsync(ApplicationUser user, string password)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");;
            }
            
           
            var result = await _userManager.CreateAsync(user, password);
            
            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> LoginUser(string email, string password)
        {
            if (_signInManager == null)
            {
                throw new IdentityObjectNotInitialized("SignInManager");
            }
            
            var result = await _signInManager.PasswordSignInAsync(email, password, 
                false, false);
            
            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteRole(string id)
        {
            if (_roleManager == null)
            {
                throw new IdentityObjectNotInitialized("RoleManager");
            }

            var role = await _roleManager.FindByIdAsync(id);
         
            var result = await _roleManager.DeleteAsync(role);

            return result.ToApplicationResult();
        }

        public async Task<Result> AddRoleToUser(string name)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var user = await _userManager.FindByIdAsync(_currentUserService.UserID);
            
            var result = await _userManager.AddToRoleAsync(user, name);
                
            return result.ToApplicationResult();
        }

        public async Task<Result> AddRoleToUser(string userID ,string name)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var user = await _userManager.FindByIdAsync(userID);
            
            var result = await _userManager.AddToRoleAsync(user, name);
                
            return result.ToApplicationResult();
        }

        public async void Logout()
        {
            if (_signInManager == null)
            {
                throw new IdentityObjectNotInitialized("SignInManager");
            }
            await _signInManager.SignOutAsync();
        }

        public async Task<Result> CreateRole(string roleName)
        {
            if (_roleManager == null)
            {
                throw new IdentityObjectNotInitialized("RoleManager");
            }
            
            ApplicationRole role = new ApplicationRole
            {
                Name = roleName
                 
            };

            var result = await _roleManager.CreateAsync(role);

            return result.ToApplicationResult();

        }

        public IQueryable<ApplicationRole> GetRoles()
        {
            if (_roleManager == null)
            {
                throw new IdentityObjectNotInitialized("RoleManager");
            }


            return _roleManager.Roles;
        }

        public async Task<IQueryable<ApplicationUser>> GetUsersByRole(string name)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var roles = await _userManager.GetUsersInRoleAsync(name);

            return roles.AsQueryable();

        }

        public async Task<IList<string>> GetRolesByUser(string userID)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var user = await _userManager.FindByIdAsync(userID);

            var rolesAsync = await _userManager.GetRolesAsync(user);

            return rolesAsync;
        }
        
        public IQueryable<ApplicationUser> GetUsers()
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var users = _userManager.Users;

            return users;
        }

        public async Task<Result> DeleteUser(string id)
        {

            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var user = await _userManager.FindByIdAsync(id);

            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<Result> UpdateUser(ApplicationUser applicationUser)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }
            
            var result = await _userManager.UpdateAsync(applicationUser);

            return result.ToApplicationResult();
        }
    }
}