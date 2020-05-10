using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Identity;

namespace Application.IntegrationTests.Common
{
    using static Testing;
    public abstract class FinancialTestBase : TestBase
    {
        private string _projectName = "Create";


        protected async Task<string> CreateFinancialProject()
        {
            /*
            var user = new ApplicationUser()
            {
                UserName = "Dummy@Dummy",
                Email = "Dummy@Dummy",
                PasswordHash = "asdahashed!"
            };
            */
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

        protected async Task<string> CreateReceipt(string projectID)
        {
            var createCommand = new CreateReceiptsCommand
            {
                FinancialProjectId = projectID,
                Total = 0,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            return await SendAsync(createCommand);
        }
        
        protected async Task<string> CreateReceipt()
        {
            var projectID = await CreateFinancialProject();
            
            var createCommand = new CreateReceiptsCommand
            {
                FinancialProjectId = projectID,
                Total = 0,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            return await SendAsync(createCommand);
        }
    }
}