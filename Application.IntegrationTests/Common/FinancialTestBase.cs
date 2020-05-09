﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Domain.Entities.Identity;

namespace Application.IntegrationTests.Common
{
    using static Testing;
    public abstract class FinancialTestBase : TestBase
    {

        protected async Task<string> CreateFinancialProject()
        {
            var user = await RunAsDefaultUserAsync();
            await AddAsync(user);
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Name = "Create",
                Users = new List<ApplicationUser>
                {
                    user
                }
            };

            return await SendAsync(createCommand);
        }
    }
}