using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
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
        protected readonly string ProjectName = "Create";

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
                Title = ProjectName,
                Users = new List<ApplicationUser>
                {
                    User,
                    SecondUser
                }
            };

            var result = await SendAsync(createCommand);
            
            return result.Id;
        }
        
        protected async Task<string> CreateFinancialProject(ApplicationUser user)
        {
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = ProjectName,
                Users = new List<ApplicationUser>
                {
                    User,
                    SecondUser,
                    user
                }
            };

            var result = await SendAsync(createCommand);
            
            return result.Id;
        }

        protected async Task<string> CreateReceipt(string projectId)
        {
            var createCommand = new CreateReceiptCommand
            {
                FinancialProjectId = projectId,
                Location = "Title",
                DateVisited = DateTime.Now,
                
            };

            return await SendAsync(createCommand);
        }
        
        protected async Task<string> CreateReceipt()
        {
            var projectId = await CreateFinancialProject();
            
            var createCommand = new CreateReceiptCommand
            {
                FinancialProjectId = projectId,
                Location = "Title",
                DateVisited = DateTime.Now
            };

            return await SendAsync(createCommand);
        }

        protected ReceiptItemDto GetReceiptItem(int count)
        {
            return new ReceiptItemDto
            {
                Price = 100,
                Count = count,
                Id = Guid.NewGuid().ToString(),
                ItemGroup = new ItemGroupDto{Value = 0},
                Users = new List<UserDto>
                {
                    new UserDto
                    {
                        Id = User.Id
                    },
                    new UserDto
                    {
                        Id = SecondUser.Id
                    }
                }
                    
            };
        }
        
    }
}