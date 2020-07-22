using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Areas.Financial.Common;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Areas.UserManagement.Models;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CoolWebsite.Areas.Financial.Controller
{
    [Area("Financial")]
    [Authorize(Roles = "Financial, Admin")]
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
        public async Task<ActionResult> CreateProject(CreateFinancialProjectModel model)
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
                AddUserModel = new AddUserModel
                {
                    UserSelectListItems = SelectListHandler.CreateFromUsers(_identity.GetUsers().ToList(), _currentUserService.UserId),
                    CurrentUserName = await _identity.GetUserNameAsync(_currentUserService.UserId),
                    CurrentUserId = _currentUserService.UserId
                }
            };
            
            return PartialView("Partial/CreateFinancialProjectModal", model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteFinancialProjectCommand
            {
                Id = id
            };

            try
            {
                await Mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (ValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
       

    }
}