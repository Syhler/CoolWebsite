using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.Common.Models;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoolWebsite.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<IdentityService> _logger;

        private readonly string? _ip;
        private readonly string? _userAgent;
        
        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ICurrentUserService currentUserService,
            ILogger<IdentityService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _logger = logger;

            if (httpContextAccessor == null) return;
            
            _ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
            _userAgent = httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        }
        
        public async Task<string> GetUserNameAsync(string userId)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");;
            }

            var result = await _userManager.FindByIdAsync(userId);

            if (result == null) {
                return string.Empty;
            }
            
            return result.FirstName + " " + result.LastName;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<(Result result, string userId)> CreateUserAsync(ApplicationUser user, string password)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");;
            }
            
           
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("CoolWebsite CreateUser : By {CurrentUserId} {@Timestamp} {@UserAgent} {@Ip} \n Created User Id : {@UserId} ", 
                    _currentUserService.UserId, DateTime.Now.ToString(CultureInfo.CurrentCulture), _userAgent, _ip, user.Id);
            }
            else
            {
                _logger.LogError("CoolWebsite CreateUser FAILED : By {CurrentUserId} {@Timestamp} {@UserAgent} {@Ip} \n Created User Id : {@UserId} ", 
                    _currentUserService.UserId, DateTime.Now.ToString(CultureInfo.CurrentCulture), _userAgent, _ip, user.Id);
            }
            
            

            
            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> LoginUser(string email, string password, bool persistent)
        {
            if (_signInManager == null)
            {
                throw new IdentityObjectNotInitialized("SignInManager");
            }
            
            var signInResult = await _signInManager.PasswordSignInAsync(email, password, 
                persistent, false);

            var user = await _userManager.FindByEmailAsync(email);

            

            if (signInResult.Succeeded)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if (claims.FirstOrDefault(x => x.Type == "FirstName") == null || 
                    claims.FirstOrDefault(x => x.Type == "LastName") == null ||
                    claims.FirstOrDefault(x => x.Type == "UserId") == null)
                {
                    await _userManager.AddClaimAsync(user, new Claim("FirstName", user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim("LastName", user.LastName));
                    await _userManager.AddClaimAsync(user, new Claim("UserId", user.Id));
                    await _signInManager.RefreshSignInAsync(user);
                }
                _logger.LogInformation("CoolWebsite LoginUser : {UserId} {Email} {@Timestamp} {@UserAgent} {@Ip}", user.Id, email, DateTime.Now.ToString(CultureInfo.CurrentCulture), _userAgent, _ip);
               
            }
            else
            {
                _logger.LogError("CoolWebsite LoginUser FAILED : {UserId} {Email} {@Timestamp} {@UserAgent} {@Ip}", user.Id, email, DateTime.Now.ToString(CultureInfo.CurrentCulture), _userAgent, _ip);
            }
            
            return signInResult.ToApplicationResult();

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

            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            
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
            
            if (result.Succeeded)
            {
                _logger.LogInformation("CoolWebsite Delete user : By {CurrentUserId} {@Timestamp} {@UserAgent} {@Ip} \n Deleted User Id : {@UserId} ", 
                    _currentUserService.UserId, DateTime.Now.ToString(CultureInfo.CurrentCulture), _userAgent, _ip, user.Id);
            }
            else
            {
                _logger.LogError("CoolWebsite Delete user FAILED : By {CurrentUserId} {@Timestamp} {@UserAgent} {@Ip} \n Deleted User Id : {@UserId} ", 
                    _currentUserService.UserId, DateTime.Now.ToString(CultureInfo.CurrentCulture), _userAgent, _ip, user.Id);
            }
           

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

        public async Task<IList<ApplicationUser>> GetUsersByIds(IList<string> ids)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");

            }
            var users = new List<ApplicationUser>();

            foreach (var id in ids)
            {
                users.Add(await _userManager.FindByIdAsync(id));
            }

            return users;
        }

        public async Task<Result> UpdateUser(UpdateApplicationUser updateApplicationUser)
        {
            if (_userManager == null)
            {
                throw new IdentityObjectNotInitialized("UserManager");
            }

            var user = await _userManager.FindByIdAsync(updateApplicationUser.Id);
            user.FirstName = updateApplicationUser.FirstName;
            user.LastName = updateApplicationUser.LastName;
            user.Email = updateApplicationUser.Email;
            user.UserName = updateApplicationUser.Email;

            if (!string.IsNullOrWhiteSpace(updateApplicationUser.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _userManager.ResetPasswordAsync(user, token, updateApplicationUser.Password);
            }

            if (!string.IsNullOrWhiteSpace(updateApplicationUser.RoleName))
            {
                await _userManager.AddToRoleAsync(user, updateApplicationUser.RoleName);

            }
            
            var result = await _userManager.UpdateAsync(user);

            return result.ToApplicationResult();
        }
    }
}