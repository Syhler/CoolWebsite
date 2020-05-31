﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
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
        public async Task<ActionResult> CreateProject(CreateFinancialProjectModel model)
        {

            var command = new CreateFinancialProjectCommand
            {
                Title = model.Name,
                Users = await _identity.GetUsersByIds(model.Users)
            };

            try
            {
                var project = await Mediator.Send(command);
                return PartialView("Partial/FinancialProjectCard", project);
            }
            catch (ValidationException e)
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetModal()
        {
            var model = new CreateFinancialProjectModel
            {
                UsersDropdown = CreateSelectedList(_identity.GetUsers()),
                CurrentUserName = await _identity.GetUserNameAsync(_currentUserService.UserID),
                CurrentUserId = _currentUserService.UserID
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
        
        
        private List<SelectListItem> CreateSelectedList(IQueryable<ApplicationUser> models)
        {
            var listOfSelectedItem = new List<SelectListItem>();

            foreach (var user in models)
            {
                if (user.Id == _currentUserService.UserID)
                {
                    continue;
                }
                
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