using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;

namespace Application.IntegrationTests.Common
{
    using static Testing;
    public abstract class FinancialTestBase : TestBase
    {
        private string _projectName = "Create";


        protected async Task<string> CreateFinancialProject()
        {
            var user = await RunAsDefaultUserAsync();
            await AddAsync(user);
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = _projectName,
                Users = new List<ApplicationUser>
                {
                    user
                }
            };

            return await SendAsync(createCommand);
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
            var user = await RunAsUserAsync("test@test", "tesada123!");

            await AddAsync(user);
            
            var createCommand = new CreateIndividualReceiptCommand
            {
                Total = 0,
                ReceiptId = receiptId,
                UserId = user.Id
            };

            return await SendAsync(createCommand);
        }
        
        protected async Task<IndividualReceipt> CreateIndividualReceipt()
        {
            var receiptId = await CreateReceipt();
            var user = await RunAsUserAsync("test@test", "tesada123!");

            await AddAsync(user);
            
            var createCommand = new CreateIndividualReceiptCommand
            {
                Total = 0,
                ReceiptId = receiptId,
                UserId = user.Id
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