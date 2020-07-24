using System;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetArchiveFinancialProjects;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    
    public class GetArchiveFinancialProjectsByUserQueryTests : FinancialTestBase
    {
        //Successfully
        [Test]
        public async Task Handle_ValidUserId_ShouldReturnListOfFinancialProjects()
        {
            var project = await CreateFinancialProject();

            var deleteCommand = new DeleteFinancialProjectCommand
            {
                Id = project
            };

            await SendAsync(deleteCommand);
            
            var query = new GetArchiveFinancialProjectsByUserQuery();

            var entity = await SendAsync(query);

            entity.Should().NotBeNull();
            entity.Count.Should().Be(1);
            entity.First().Id.Should().Be(project);
            entity.First().Users.FirstOrDefault(x => x.Id == User.Id).Should().NotBeNull();
            entity.First().Deleted.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}