﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.UpdateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using CoolWebsite.Areas.Financial.Common;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CoolWebsite.Areas.Financial.Controller
{
    [Area("Financial")]
    [Authorize(Roles = "Admin")]
    public class ProjectController : MediatorController
    {
        private readonly ICurrentUserService _currentUserService;

        public ProjectController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        // GET
        public async Task<IActionResult> Index(string id)
        {
            var query = new GetFinancialProjectByIdQuery {ProjectId = id};
            
            var model = await Mediator.Send(query);

            model.Users = model.Users.Where(x => x.Id != _currentUserService.UserID).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateReceipt(string id)
        {

            var financialQuery = new GetUsersFromFinancialProjectQuery() {FinancialProjectId = id};

            var userDtos = await Mediator.Send(financialQuery);
          
            var model = new CreateReceiptModel
            {
                FinancialProjectId = id,
                CreateReceiptItemVm = await CreateReceiptItemVm(userDtos)
            };
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditReceipt(string id, string financialProjectId)
        {
            var query = new GetReceiptByIdQueryVm
            {
                ReceiptId = id
            };

            var receiptDto = await Mediator.Send(query);

            var usersQuery = new GetUsersFromFinancialProjectQuery{ FinancialProjectId = financialProjectId};

            var userDtos = await Mediator.Send(usersQuery);
            
            var model = new CreateReceiptModel
            {
                ReceiptDto =  receiptDto,
                FinancialProjectId = financialProjectId,
                CreateReceiptItemVm = await CreateReceiptItemVm(userDtos)
            };
            
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditReceiptPost(CreateReceiptModel model)
        {

            var command = new UpdateReceiptsCommand
            {
                Id = model.ReceiptDto.Id,
                Location = model.ReceiptDto.Location,
                ItemDtos = model.ReceiptDto.Items,
                Datevisited = model.ReceiptDto.DateVisited,
                Note = model.ReceiptDto.Note
            };

            await Mediator.Send(command);

            return Json(new {result = "Redirect", url = Url.Action("Index", "Project", new {id = model.FinancialProjectId})});
        }
        
        

        [HttpPost]
        public async Task<IActionResult> GetReceiptItemPartialView(ReceiptItemVm vm)
        {
            vm.UniqueIdentifier = Guid.NewGuid();
            
            vm.ReceiptItem.Price = Math.Round(vm.ReceiptItem.Price, 2);
            
            return PartialView("Partial/ReceiptItemPartialView", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceiptPost(CreateReceiptModel model)
        {
            
            var command = new CreateReceiptsCommand
            {
                Total = model.ReceiptDto.Items.Select(x => x.Price).Sum(),
                BoughtAt = model.ReceiptDto.DateVisited,
                FinancialProjectId = model.FinancialProjectId,
                Title = model.ReceiptDto.Location,
                Note = model.ReceiptDto.Note
            };

            var receiptId = await Mediator.Send(command);

            foreach (var receiptItemModel in model.ReceiptDto.Items)
            {
                var createReceiptItemCommand = new CreateReceiptItemCommand
                {
                    Name = "Receipt",
                    Count = receiptItemModel.Count,
                    ItemGroup = receiptItemModel.ItemGroup.Value,
                    ReceiptId = receiptId,
                    Price = receiptItemModel.Price,
                    UsersId = receiptItemModel.Users.Select(x => x.Id).ToList()
                };

                await Mediator.Send(createReceiptItemCommand);
            }
            
            //redirect to index
            return Json(new {result = "Redirect", url = Url.Action("Index", "Project", new {id = model.FinancialProjectId})});
        }

        public async Task<IActionResult> DeleteReceipt(string id, string projectId)
        {
            var command = new DeleteReceiptsCommand
            {
                Id = id
            };

            await Mediator.Send(command);


            return RedirectToAction("Index", "Project", new {id = projectId});
        }
        
        
        [HttpGet]
        public async Task<IActionResult> CreateReceiptItemModal(string financialProjectId)
        {
            return View("Partial/CreateReceiptItemModal", null);
        }
        
      

        private async Task<CreateReceiptItemVm> CreateReceiptItemVm(IList<UserDto> users)
        {
            var itemGroupQuery = new GetItemGroupQuery();

            var itemGroups = await Mediator.Send(itemGroupQuery);
            
            return  new CreateReceiptItemVm
            {
                AddUserModel = new AddUserModel
                {
                    UserSelectListItems = SelectListHandler.CreateFromUsers(users),
                },
                TypesSelectListItems = SelectListHandler.CreateFromItemGroup(itemGroups),
            };
        }
    }
}