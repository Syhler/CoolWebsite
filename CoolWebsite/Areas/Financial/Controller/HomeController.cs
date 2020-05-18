using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Areas.UserManagement.Models;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Controller
{
    [Area("Financial")]
    [Authorize(Roles = "Admin")]
    public class HomeController : MediatorController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identity;

        public HomeController(ICurrentUserService currentUserService, IIdentityService identity)
        {
            _currentUserService = currentUserService;
            _identity = identity;
        }

        // GET
        public async Task<ViewResult> Index()
        {
            var model = await Mediator.Send(new GetAllFinancialProjectQuery());
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateFinancialProjectModel model)
        {

            var command = new CreateFinancialProjectCommand
            {
                Title = model.Name,
                Users = await _identity.GetUsersByIds(model.Users)
            };

            var project = await Mediator.Send(command);

            return PartialView("Partial/FinancialProjectCard", project);
        }

        [HttpGet]
        public async Task<IActionResult> GetModal()
        {
            var model = new CreateFinancialProjectModel
            {
                UsersDropdown = CreateSelectedList(_identity.GetUsers())
            };
            
            return PartialView("Partial/CreateFinancialProjectModal", model);
        }
        
        private List<SelectListItem> CreateSelectedList(IQueryable<ApplicationUser> models)
        {
            var listOfSelectedItem = new List<SelectListItem>();

            foreach (var user in models)
            {
                listOfSelectedItem.Add(new SelectListItem
                {
                    Value = user.Id,
                    Text = user.FirstName +" " + user.LastName
                });
            }

            return listOfSelectedItem;
        }
    }
}