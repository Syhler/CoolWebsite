using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolWebsite.Areas.Financial.Controller
{
    [Area("Financial")]
    [Authorize(Roles = "Admin")]
    public class IndexController : MediatorController
    {
        private readonly ICurrentUserService _currentUserService;

        public IndexController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }


        // GET
        public async Task<ViewResult> Index()
        {
            var vm = await Mediator.Send(new GetAllFinancialProjectQuery());
            
            return View(vm);
        }
    }
}