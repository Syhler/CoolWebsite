using System.Threading.Tasks;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoolWebsite.Controllers
{
    public class AuthController : Controller
    {
        private readonly IdentityService _identityService;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _identityService = new IdentityService(userManager, signInManager);
        }
    
    
        // GET
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var result = await _identityService.LoginUser(model.Email, model.Password);

            if (result.Succeeded)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Logout()
        {
            _identityService.Logout();
            return RedirectToAction("Index");
        }
    }
}