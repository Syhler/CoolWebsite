using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolWebsite.Areas.Financial.Controller
{
    
    [Area("Financial")]
    [Authorize(Roles = "Admin")]
    public class ProjectController : MediatorController
    {
        // GET
        public async Task<IActionResult> Index(string id)
        {
            var query = new GetFinancialProjectByIdQuery{ProjectId = id};
            var model = await Mediator.Send(query);
            model.Receipts.Add(new ReceiptsDto{Location = "hej"});
            model.Receipts.Add(new ReceiptsDto{Location = "hej"});
            model.Receipts.Add(new ReceiptsDto{Location = "hej"});
            model.Receipts.Add(new ReceiptsDto{Location = "hej"});
            model.Receipts.Add(new ReceiptsDto{Location = "hej"});
            model.Receipts.Add(new ReceiptsDto{Location = "hej"});
           
            
            return View(model);
        }
    }
}