using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
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
            
            var createReceipt = new CreateReceiptsCommand
            {
                Total = 2000,
                FinancialProjectId = project.Id,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            var receiptId = await SendAsync(createReceipt);
            
            var createIndividualReceipt = new CreateIndividualReceiptCommand
            {
                ReceiptId = receiptId,
                Total = 123.123,
                UserId = User.Id
            };

            var individualReceiptId = await SendAsync(createIndividualReceipt);

            var query = new GetAllFinancialProjectQuery(){ };

            var model = await SendAsync(query);

            
            model.Should().NotBeNull();
            model.FinancialProjects.First().Title.Should().Be(createCommand.Title);
            model.FinancialProjects.First().Receipts.First().Id.Should().Be(receiptId);
            model.FinancialProjects.First().Id.Should().Be(project.Id);
            model.FinancialProjects.First().Receipts.First().Total.Should().Be(createReceipt.Total);
            model.FinancialProjects.First().Receipts.First().Title.Should().Be(createReceipt.Title);
            model.FinancialProjects.First().Receipts.First().BoughtAt.Should().BeCloseTo(DateTime.Now, 1000);
            model.FinancialProjects.First().Receipts.First().IndividualReceipts.First().Total.Should().Be(createIndividualReceipt.Total);
            //TODO(maybe in the future look at this)
            //model.FinancialProjects.First().Receipts.First().IndividualReceipts.First().Receipt.Id.Should().Be(createIndividualReceipt.ReceiptId);
            model.FinancialProjects.First().Receipts.First().IndividualReceipts.First().Id.Should().Be(individualReceiptId);
            model.FinancialProjects.First().Receipts.First().IndividualReceipts.First().User.Id.Should().Be(User.Id);


        }
    }
}