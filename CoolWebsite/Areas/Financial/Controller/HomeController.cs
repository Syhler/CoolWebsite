﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.UpdateFinancialProject;
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
            var model = await Mediator.Send(new GetFinancialProjectQuery());
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject(CreateFinancialProjectModel model)
        {

            var command = new CreateFinancialProjectCommand
            {
                Title = model.Name,
                Users = await _identity.GetUsersByIds(model.Users),
                Description = model.Description
            };

            var project = await Mediator.Send(command);
            return PartialView("Partial/FinancialProjectCard", project);

        }

        
        [HttpGet]
        public async Task<IActionResult> GetEditModal(string id)
        {
            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = id
            };

            var project = await Mediator.Send(query);
            
            
            var model = new EditFinancialProjectModel
            {
                Name = project.Title,
                Description = project.Description,
                AddUserModel = await GetAddUserModel(),
                Id = id
            };
            model.AddUserModel.ExistingUsers = project.Users;

            foreach (var projectUser in project.Users)
            {
                var duplicate = model.AddUserModel.UserSelectListItems.FirstOrDefault(x => x.Value == projectUser.Id);

                if (duplicate != null)
                {
                    model.AddUserModel.UserSelectListItems?.Remove(duplicate);
                }
            }

            return PartialView("Partial/EditFinancialProjectModal", model);
        }

        [HttpPost]
        public async Task UpdateFinancialProject(EditFinancialProjectModel model)
        {
            var command = new UpdateFinancialProjectCommand
            {
                Users = model.Users,
                Name = model.Name,
                Id = model.Id ?? "",
                Description = model.Description
            };

            await Mediator.Send(command);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetModal()
        {
            if (_currentUserService.UserId == null) {
                throw new IdentityCurrentUserIdNotSet();
            }
            
            var model = new CreateFinancialProjectModel
            {
                AddUserModel = await GetAddUserModel()
            };
            
            return PartialView("Partial/CreateFinancialProjectModal", model);
        }

        public async Task<IActionResult> ArchiveProject(string id)
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

        private async Task<AddUserModel> GetAddUserModel()
        {
            return new AddUserModel
            {
                UserSelectListItems =
                    SelectListHandler.CreateFromUsers(_identity.GetUsers().ToList(), _currentUserService.UserId),
                CurrentUserName = await _identity.GetUserNameAsync(_currentUserService.UserId),
                CurrentUserId = _currentUserService.UserId
            };
        }
        
        
       

    }
}