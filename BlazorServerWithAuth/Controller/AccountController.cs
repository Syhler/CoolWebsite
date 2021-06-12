using Microsoft.AspNetCore.Mvc;

namespace BlazorServerWithAuth.Controller
{
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}