using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    public class GetFinancialProjectsByUserTests : FinancialTestBase
    {
        //TODO(needs be check if recipets gets returned aswell. DO this after implementing test for reciepts)
        
        [Test]
        public async Task Handle_ValidUserId_ShouldReturnProject()
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

            await SendAsync(createCommand);

            var query = new GetFinancialProjectsByUserQuery
            {
                UserId = user.Id
            };

            var model = await SendAsync(query);

            model.Should().NotBeNull();
            model.FinancialProjects.First().Users.First().Id.Should().Be(user.Id);
            model.FinancialProjects.First().Title.Should().Be(createCommand.Name);
        }

        [Test]
        public async Task Handle_InvalidUserId_ShouldThrowNotFoundException()
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

            await SendAsync(createCommand);

            var query = new GetFinancialProjectsByUserQuery
            {
                UserId = "ads"
            };
            
            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<NotFoundException>();

            

        }

        [Test]
        public async Task Handle_EmptyUserId_ShouldThrowValidationException()
        {
            var query = new GetFinancialProjectsByUserQuery();

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }
    }
}