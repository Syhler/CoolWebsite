using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        } 
        
        // public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        // {
        //     _userManager = userManager;
        //     _roleManager = roleManager;
        // }
        //
        // public IdentityService(RoleManager<ApplicationRole> roleManager)
        // {
        //     _roleManager = roleManager;
        // }
        //
        // public IdentityService(UserManager<ApplicationUser> userManager)
        // {
        //     _userManager = userManager;
        // }


        public async Task<string> GetUserNameAsync(string userID)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> CreateUserAsync(string email, string password)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");;
            }
            
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            var result = await _userManager.CreateAsync(user, password);
            return result.ToApplicationResult();
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

        public  IQueryable<ApplicationRole> GetRoles()
        {
            if (_roleManager == null)
            {
                throw new IdentityObjectNotInitialized("RoleManager");
            }
            
            return _roleManager.Roles;
        }
    }
}