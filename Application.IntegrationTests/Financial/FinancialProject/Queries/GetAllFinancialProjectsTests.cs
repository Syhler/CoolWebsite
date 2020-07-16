using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    
    public class GetAllFinancialProjectsTests : FinancialTestBase
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
            
            var createReceipt = new CreateReceiptCommand
            {
                FinancialProjectId = project.Id,
                Location = "Title",
                DateVisited = DateTime.Now
            };

            var receiptId = await SendAsync(createReceipt);

            var query = new GetAllFinancialProjectQuery();

            var model = await SendAsync(query);

            
            model.Should().NotBeNull();
            model.FinancialProjects.First().Title.Should().Be(createCommand.Title);
            model.FinancialProjects.First().Receipts.First().Id.Should().Be(receiptId);
            model.FinancialProjects.First().Id.Should().Be(project.Id);
            model.FinancialProjects.First().Receipts.First().Location.Should().Be(createReceipt.Location);
            model.FinancialProjects.First().Receipts.FirstOrDefault(x => x.Deleted != null).Should().BeNull();
            model.FinancialProjects.First().Receipts.First().DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            model.FinancialProjects.First().Created.Should().BeCloseTo(DateTime.Now, 1000);
            model.FinancialProjects.First().Users.First().Id.Should().Be(User.Id);
            model.FinancialProjects.First().Users.Count.Should().Be(1);


        }
    }
}