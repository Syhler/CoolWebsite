using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Areas.UserManagement.Models;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using NUnit.Framework;

namespace Application.IntegrationTests.Common
{
    using static Testing;
    public abstract class FinancialTestBase : TestBase
    {
        private string _projectName = "Create";

        protected ApplicationUser User;

        [SetUp]
        public async Task CreateUser()
        {
            User = await RunAsDefaultUserAsync();
        }

        protected async Task<string> CreateFinancialProject()
        {
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = _projectName,
                Users = new List<ApplicationUser>
                {
                    User
                }
            };

            var result = await SendAsync(createCommand);
            
            return result.Id;
        }

        protected async Task<string> CreateReceipt(string projectId)
        {
            var createCommand = new CreateReceiptsCommand
            {
                FinancialProjectId = projectId,
                Total = 0,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            return await SendAsync(createCommand);
        }
        
        protected async Task<string> CreateReceipt()
        {
            var projectId = await CreateFinancialProject();
            
            var createCommand = new CreateReceiptsCommand
            {
                FinancialProjectId = projectId,
                Total = 0,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            return await SendAsync(createCommand);
        }

        
        protected async Task<string> CreateIndividualReceipt(string receiptId)
        {

            var createCommand = new CreateIndividualReceiptCommand
            {
                Total = 0,
                ReceiptId = receiptId,
                UserId = User.Id
            };

            return await SendAsync(createCommand);
        }
        
        protected async Task<IndividualReceipt> CreateIndividualReceipt()
        {
            var receiptId = await CreateReceipt();

            var createCommand = new CreateIndividualReceiptCommand
            {
                Total = 0,
                ReceiptId = receiptId,
                UserId = User.Id
            };

            var id = await SendAsync(createCommand);
            
            return new IndividualReceipt()
            {
                Id = id,
                Total = createCommand.Total,
                ReceiptId = createCommand.ReceiptId,
                UserId = createCommand.UserId
            };
        }
    }
}