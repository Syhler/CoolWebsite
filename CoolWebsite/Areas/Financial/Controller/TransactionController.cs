using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Controller
{
    [Area("Financial")]
    [Authorize(Roles = "Financial, Admin")]
    public class TransactionController : MediatorController
    {
        public async Task<IActionResult> Index()
        {
            var projects = await Mediator.Send(new GetFinancialProjectQuery());

            var model = new TransactionVm
            {
                TransactionPartialModel = new TransactionPartialModel
                {
                    PayedTransaction = await Mediator.Send(new GetPayedTransactionQuery()),
                    ReceivedTransaction = await Mediator.Send(new GetReceivedTransactionQuery()),
                },
                Projects = projects.FinancialProjects.ToList()
            };
           
            return View(model);
        }

        public async Task<IActionResult> TransactionPartial()
        {
            var projects = await Mediator.Send(new GetFinancialProjectQuery());
            
            var model = new TransactionVm
            {
                SelectedProject = "",
                TransactionPartialModel = new TransactionPartialModel
                {
                    PayedTransaction = await Mediator.Send(new GetPayedTransactionQuery()),
                    ReceivedTransaction = await Mediator.Send(new GetReceivedTransactionQuery())
                },
                Projects = projects.FinancialProjects.ToList()
            };

            return PartialView("Partial/IndexPartial", model);
        }
        
        public async Task<IActionResult> TransactionPartialByProject(string id)
        {
            var projects = await Mediator.Send(new GetFinancialProjectQuery());
            
            var model = new TransactionVm
            {
                SelectedProject = id,
                TransactionPartialModel = new TransactionPartialModel
                {
                    PayedTransaction = await Mediator.Send(new GetPayedTransactionByProjectQuery{FinancialProjectId = id}),
                    ReceivedTransaction = await Mediator.Send(new GetReceivedTransactionByProjectQuery{FinancialProjectId = id})
                },
                Projects = projects.FinancialProjects.ToList()
            };

            return PartialView("Partial/IndexPartial", model);

        }
      
    }
}