using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoolWebsite.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Controllers
{
    public class HomeController : MediatorController
    {
        private readonly IIdentityService _identityService;

        
        public HomeController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (model.Email == null || model.Password == null)
            {
                return Json(new {result="Failure", errors = "Email or password is null"});
            }
            
            var result = await _identityService.LoginUser(model.Email, model.Password, model.Persistence);

            if (!result.Succeeded) return Json(new {result="Failure", errors = result.Errors});
            
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Json(new {result="Redirect", url = Url.Action(returnUrl)});
            }

            return Json(new {result = "Redirect", url = Url.Action("Index", "Home", new {area = "Financial"})});

        }

        public void AddClaims(HttpContext httpContext, ApplicationUser user)
        {
            if (httpContext.User == null) return;
            
            var claims = new List<Claim>
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            var appIdentity = new ClaimsIdentity(claims);
            httpContext.User.AddIdentity(appIdentity);
        }
        
        [AllowAnonymous]
        public IActionResult CoolWebsiteProject()
        {
            return View();
        }
        
        
        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
      
    }
}