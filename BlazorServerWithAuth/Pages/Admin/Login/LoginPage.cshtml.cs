using System;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorServerWithAuth.Pages.Admin.Login
{
    public class LoginPage : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public LoginPage(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }


        public IActionResult OnGet()
        {
            
            if (User == null) return Page();
            if (User.Identity == null) return Page();
            if (User.Identity.IsAuthenticated) return LocalRedirect("/admin");

            return Page();
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        
        public async Task<IActionResult> OnPostLogin(string username, string password)
        {
  
            var result = await _identityService.LoginUser(username, password, false);

            if (result.Succeeded) return LocalRedirect("/admin");

            return Page();

        }

     
    }
}