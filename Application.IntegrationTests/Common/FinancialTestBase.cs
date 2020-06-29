using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
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
        protected ApplicationUser SecondUser;

        [SetUp]
        public async Task CreateUser()
        {
            User = await RunAsDefaultUserAsync();
            SecondUser = await CreateNewUser("SecondUser", "YesIndeed");
        }

        protected async Task<string> CreateFinancialProject()
        {
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = _projectName,
                Users = new List<ApplicationUser>
                {
                    User,
                    SecondUser
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
                BoughtAt = DateTime.Now,
                
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
        
    }
}