using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoolWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> CreateAccount()
        {
            var user = new ApplicationUser
            {
                UserName = "morten.syhler@gmail.com",
                Email = "morten.syhler@gmail.com",
                Color = "green"
            };

            var result = await _userManager.CreateAsync(user, "test");
            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> Login()
        {
            await _signInManager.PasswordSignInAsync("morten.syhler@gmail.com", "test", false, false);
            return RedirectToAction("Index");
        }


        public string GetCurrentUser()
        {
            var user = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return user;
        }
    }
}