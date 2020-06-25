using System;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
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

            var financialQuery = new GetFinancialProjectByIdQuery {ProjectId = id};

            var project = await Mediator.Send(financialQuery);
            
            var itemGroupQuery = new GetItemGroupQuery();

            var itemGroups = await Mediator.Send(itemGroupQuery);

            var createReceiptItemVm = new CreateReceiptItemVm
            {
                AddUserModel = new AddUserModel
                {
                    UserSelectListItems = SelectListHandler.CreateFromUsers(project.Users),
                },
                TypesSelectListItems = SelectListHandler.CreateFromItemGroup(itemGroups),
            };
            
            
            var model = new CreateReceiptModel
            {
                FinancialProjectId = id,
                CreateReceiptItemVm = createReceiptItemVm

            };
            
            return View(model);
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
                Total = model.ReceiptItemModels.Select(x => x.Price).Sum(),
                BoughtAt = model.DateVisited,
                FinancialProjectId = model.FinancialProjectId,
                Title = model.Location,
                Note = model.Note
            };

            var receiptId = await Mediator.Send(command);

            foreach (var receiptItemModel in model.ReceiptItemModels)
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
    }
}