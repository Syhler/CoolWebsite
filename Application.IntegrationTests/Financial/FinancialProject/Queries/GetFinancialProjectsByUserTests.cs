using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    public class GetFinancialProjectsByUserTests : FinancialTestBase
    {
        
        [Test]
        public async Task Handle_ValidUserId_ShouldReturnProject()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "Create",
                Users = new List<ApplicationUser>
                {
                    User
                }
            };

            var project = await SendAsync(createCommand);
            
            var createReceipt = new CreateReceiptsCommand
            {
                FinancialProjectId = project.Id,
                Location = "Title",
                DateVisited = DateTime.Now
            };

            var receiptId = await SendAsync(createReceipt);

            var query = new GetFinancialProjectsByUserQuery
            {
                UserId = User.Id
            };

            var model = await SendAsync(query);

            
            model.Should().NotBeNull();
            model.FinancialProjects.First().Users.First().Id.Should().Be(User.Id);
            model.FinancialProjects.First().Title.Should().Be(createCommand.Title);
            model.FinancialProjects.First().Receipts.First().Id.Should().Be(receiptId);
            model.FinancialProjects.First().Id.Should().Be(project.Id);
            model.FinancialProjects.First().Receipts.First().Location.Should().Be(createReceipt.Location);
            model.FinancialProjects.First().Receipts.First().DateVisited.Should().BeCloseTo(DateTime.Now, 1000);

        }

        [Test]
        public async Task Handle_InvalidUserId_ShouldThrowNotFoundException()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "Create",
                Users = new List<ApplicationUser>
                {
                    User
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