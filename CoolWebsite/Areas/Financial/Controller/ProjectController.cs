using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries;
using CoolWebsite.Areas.Financial.Common;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            model.Receipts.Add(new ReceiptsDto {Location = "hej"});
            model.Receipts.Add(new ReceiptsDto {Location = "hej"});
            model.Receipts.Add(new ReceiptsDto {Location = "hej"});
            model.Receipts.Add(new ReceiptsDto {Location = "hej"});
            model.Receipts.Add(new ReceiptsDto {Location = "hej"});
            model.Receipts.Add(new ReceiptsDto {Location = "hej"});


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateReceipt(string id)
        {
            return View(new CreateReceiptModel {FinancialProjectId = id});
        }

        [HttpPost]
        public async Task<IActionResult> GetReceiptItemPartialView(ReceiptItemModel model)
        {
            return PartialView("Partial/ReceiptItemPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceiptPost(CreateReceiptModel model)
        {
            return RedirectToAction("Index", new {id = model.FinancialProjectId});
        }

        [HttpGet]
        public async Task<IActionResult> CreateReceiptItemModal(string financialProjectId)
        {
            var financialQuery = new GetFinancialProjectByIdQuery {ProjectId = financialProjectId};

            var project = await Mediator.Send(financialQuery);
            
            var itemGroupQuery = new GetItemGroupQuery();

            var itemGroups = await Mediator.Send(itemGroupQuery);

            var model = new CreateReceiptItemVm
            {
                AddUserModel = new AddUserModel
                {
                    UserSelectListItems = SelectListHandler.CreateFromUsers(project.Users),
                },
                TypesSelectListItems = SelectListHandler.CreateFromItemGroup(itemGroups),
            };

            return View("Partial/CreateReceiptItemModal", model);
        }
    }
}