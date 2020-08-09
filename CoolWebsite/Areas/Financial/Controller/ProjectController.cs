using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using CoolWebsite.Areas.Financial.Common;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Domain.Enums;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CoolWebsite.Areas.Financial.Controller
{
    [Area("Financial")]
    [Authorize(Roles = "Financial, Admin")]
    public class ProjectController : MediatorController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public ProjectController(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        // GET
        public async Task<IActionResult> Index(string id)
        {
            var query = new GetFinancialProjectByIdQuery {ProjectId = id};

            var model = await Mediator.Send(query);

            model.Users = model.Users.Where(x => x.Id != _currentUserService.UserId).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task PayTransaction(PayTransaction model)
        {
            var command = new CreateTransactionCommand
            {
                TransactionType = TransactionType.FinancialReceipts,
                ToUserId = model.ToUserId,
                Amount = model.Amount,
                FinancialProjectId = model.FinancialProjectId
            };

            await Mediator.Send(command);
        }

        [HttpPost]
        public async Task<IActionResult> PayTransactionMobilePay(PayTransaction model)
        {
            var user = await _identityService.GetUserById(model.ToUserId);
            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                return Json(new
                {
                    respone = "Error",
                    result = user.FirstName + " " + user.LastName + " have not registered a phone number"
                });
            }

            var command = new CreateTransactionCommand
            {
                TransactionType = TransactionType.FinancialReceiptsMobilePay,
                ToUserId = model.ToUserId,
                Amount = model.Amount,
                FinancialProjectId = model.FinancialProjectId
            };

            await Mediator.Send(command);


            var mobilePayDeepLink = MobilePayDeepLink.GenerateUrl(user, model.Amount);

            return Json(new {response = "Succeed", result = mobilePayDeepLink});
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
            var query = new GetReceiptByIdQuery
            {
                ReceiptId = id
            };

            var receiptDto = await Mediator.Send(query);

            var usersQuery = new GetUsersFromFinancialProjectQuery {FinancialProjectId = financialProjectId};

            var userDtos = await Mediator.Send(usersQuery);

            var model = new CreateReceiptModel
            {
                ReceiptDto = receiptDto,
                FinancialProjectId = financialProjectId,
                CreateReceiptItemVm = await CreateReceiptItemVm(userDtos)
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditReceiptPost(CreateReceiptModel model)
        {
            //update receipt

            var command = new UpdateReceiptCommand
            {
                Id = model.ReceiptDto.Id,
                Location = model.ReceiptDto.Location,
                ItemDtos = model.ReceiptDto.Items, //maybe handle it with a updateReceiptItem call - problem how do we know if a item have been added?
                DateVisited = model.ReceiptDto.DateVisited,
                Note = model.ReceiptDto.Note
            };

            //await Mediator.Send(command);

            //get already existing receipt items
            //figure out if item need to be updated or not
            var itemQuery = new GetReceiptItemByReceiptIdQuery
            {
                ReceiptId = model.ReceiptDto.Id
            };

            var alreadyExistingItems = await Mediator.Send(itemQuery);

            //delete receipt item - own request
            var receiptItemsToDelete = alreadyExistingItems
                .Where(x => model.ReceiptDto.Items
                    .All(y => y.Id != x.Id))
                .ToList();
            
            foreach (var itemDto in receiptItemsToDelete)
            {
                var deleteCommand = new DeleteReceiptItemCommand
                {
                    Id = itemDto.Id!,
                    FinancialProjectId = model.FinancialProjectId
                };

                await Mediator.Send(deleteCommand);
            }
            
            //Create receipt item
            var receiptItemToBeCreated = model.ReceiptDto.Items
                .Where(x => alreadyExistingItems
                    .All(y => y.Id != x.Id))
                .ToList();
            
            foreach (var receiptItemDto in receiptItemToBeCreated)
            {
                var createCommand = new CreateReceiptItemCommand
                {
                    Name = "",
                    Count = receiptItemDto.Count,
                    Price = receiptItemDto.Price,
                    ItemGroup = receiptItemDto.ItemGroup.Value,
                    ReceiptId = model.ReceiptDto.Id,
                    UserIds = receiptItemDto.Users.Select(x => x.Id).ToList()!
                };
                
                await Mediator.Send(createCommand);
            }

            //update receipt item - own request
            var receiptItemsToUpdate = model.ReceiptDto.Items
                .Where(x => receiptItemsToDelete
                    .All(y => y.Id != x.Id) &&
                            receiptItemToBeCreated.All(q => q.Id != x.Id))
                .ToList();
            
            foreach (var receiptItemDto in receiptItemsToUpdate)
            {
                var updateCommand = new UpdateReceiptItemCommand
                {
                    Id = receiptItemDto.Id ?? "",
                    Count = receiptItemDto.Count,
                    ItemGroup = receiptItemDto.ItemGroup.Value,
                    Price = receiptItemDto.Price,
                    UserDtos = receiptItemDto.Users.ToList(),
                    FinancialProjectId = model.FinancialProjectId
                };
                await Mediator.Send(updateCommand);
            }
           


            return Json(
                new
                {
                    result = "Redirect",
                    url = Url.Action("Index", "Project", new {id = model.FinancialProjectId})
                });
        }


        [HttpPost]
        public IActionResult GetReceiptItemPartialView(ReceiptItemVm vm)
        {
            vm.UniqueIdentifier = string.IsNullOrWhiteSpace(vm.ReceiptItem.Id)
                ? Guid.NewGuid()
                : Guid.Parse(vm.ReceiptItem.Id);


            vm.ReceiptItem.Price = Math.Round(vm.ReceiptItem.Price, 2);

            return PartialView("Partial/ReceiptItemPartialView", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceiptPost(CreateReceiptModel model)
        {
            var command = new CreateReceiptCommand
            {
                DateVisited = model.ReceiptDto.DateVisited,
                FinancialProjectId = model.FinancialProjectId,
                Location = model.ReceiptDto.Location,
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
                    UserIds = receiptItemModel.Users.Select(x => x.Id).ToList()!
                };

                await Mediator.Send(createReceiptItemCommand);
            }

            //redirect to index
            return Json(new
                {result = "Redirect", url = Url.Action("Index", "Project", new {id = model.FinancialProjectId})});
        }

        [HttpPost]
        public async Task ArchiveReceipt(string id)
        {
            var command = new DeleteReceiptCommand
            {
                Id = id
            };

            await Mediator.Send(command);
        }


        [HttpGet]
        public IActionResult CreateReceiptItemModal(string financialProjectId)
        {
            return View("Partial/CreateReceiptItemModal", null);
        }


        private async Task<CreateReceiptItemVm> CreateReceiptItemVm(IList<UserDto> users)
        {
            var itemGroupQuery = new GetItemGroupQuery();

            var itemGroups = await Mediator.Send(itemGroupQuery);

            return new CreateReceiptItemVm
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