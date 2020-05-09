using System.Collections.Generic;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
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
                Name = _projectName,
                Users = new List<ApplicationUser>
                {
                    user
                }
            };

            return await SendAsync(createCommand);
        }
    }
}