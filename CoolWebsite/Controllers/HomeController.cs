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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Controllers
{
    public class HomeController : MediatorController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        
        public HomeController(ILogger<HomeController> logger, 
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityService identityService,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
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

        public string GetCurrentUser()
        {
            var user = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return user;
        }
      
    }
}