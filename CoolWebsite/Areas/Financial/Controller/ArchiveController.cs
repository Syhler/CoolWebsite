using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.ActivateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetArchiveFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.ActivateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolWebsite.Areas.Financial.Controller
{
    
    [Area("Financial")]
    [Authorize(Roles = "Financial, Admin")]
    public class ArchiveController : MediatorController
    {
        
        
        public async Task<IActionResult> Index()
        {
            var model = new ArchiveVm
            {
                FinancialProjects = await Mediator.Send(new GetArchiveFinancialProjectsByUserQuery()),
                Receipts = await Mediator.Send(new GetArchiveReceiptsByUserQuery())
            };
            
            return View(model);
        }

        public async Task<IActionResult> ActivateReceipt(string id)
        {
            var command = new ActivateReceiptCommand{ReceiptId = id};

            await Mediator.Send(command);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateFinancialProject(string id)
        {
            var command = new ActivateFinancialProjectCommand{ProjectId = id};

            await Mediator.Send(command);

            return RedirectToAction("Index");
        }
    }
}