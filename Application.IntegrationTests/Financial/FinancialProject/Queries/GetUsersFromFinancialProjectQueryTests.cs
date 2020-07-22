using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;

    public class GetUsersFromFinancialProjectQueryTests : FinancialTestBase
    {
        //works
        [Test]
        public async Task Handle_ValidId_ShouldReturnUsers()
        {
            var project = await CreateFinancialProject();
            
            var query = new GetUsersFromFinancialProjectQuery
            {
                FinancialProjectId = project
            };

            var model = await SendAsync(query);

            model.Should().NotBeNull();
            model.Should().HaveCount(2);
            model.Should().OnlyHaveUniqueItems(x => x.Id);
            model.FirstOrDefault(x => x.Id == User.Id).Should().NotBeNull();
            model.FirstOrDefault(x => x.Id == SecondUser.Id).Should().NotBeNull();
        }
        
        
        //Validation
        [Test]
        public void Handle_EmptyId_ShouldThrowValidationException()
        {
            
            var query = new GetUsersFromFinancialProjectQuery
            {
                FinancialProjectId = ""
            };



            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_NullId_ShouldThrowValidationException()
        {
            var query = new GetUsersFromFinancialProjectQuery
            {
                FinancialProjectId = null
            };



            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }
        
        //WrongId
        [Test]
        public void Handle_WrongId_ShouldThrowNotFoundException()
        {
            var query = new GetUsersFromFinancialProjectQuery
            {
                FinancialProjectId = "asdasdas"
            };


            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<NotFoundException>();
        }


    }
}