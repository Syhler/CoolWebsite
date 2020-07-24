using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetArchiveFinancialProjects;
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
    }
}