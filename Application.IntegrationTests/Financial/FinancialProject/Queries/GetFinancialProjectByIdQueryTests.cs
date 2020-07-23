using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    
    public class GetFinancialProjectByIdQueryTests : FinancialTestBase
    {
        
        
        [Test]
        public async Task Handle_ValidProjectId_ShouldReturnProject()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "test",
                Users = new List<ApplicationUser>
                {
                    User,
                    SecondUser
                }
            };
            
            var project = await SendAsync(createCommand);


            var receiptId = await CreateReceipt(project.Id);
            
            var createReceipt = new CreateReceiptCommand
            {
                FinancialProjectId = project.Id,
                Location = "Title",
                DateVisited = DateTime.Today.AddDays(-5),

            };

            await SendAsync(createReceipt);
            
            var receiptItemCommand = new CreateReceiptItemCommand
            {
                Count = 5,
                Price = 2,
                Name = "test",
                ItemGroup = (int) ItemGroup.Essentials,
                ReceiptId = receiptId,
                UserIds = new List<string> {SecondUser.Id}
            };

            var receiptItemId = await SendAsync(receiptItemCommand);
            
            
            
            
            
            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = project.Id
            };

            var model = await SendAsync(query);

            model.Should().NotBeNull();
            model.Receipts.First().Id.Should().Be(receiptId);
            model.Receipts.Count.Should().Be(2);
            model.Receipts.First().Location.Should().Be("Title");
            model.Receipts.First().DateVisited.Should().BeCloseTo(DateTime.Now, 10000);
            model.Receipts.First().Items.First().Id.Should().Be(receiptItemId);
            model.Receipts.First().Items.First().Users.FirstOrDefault(x => x.Id == SecondUser.Id).Should().NotBeNull();
            model.Receipts.First().Items.First().ItemGroup.Value.Should().Be(receiptItemCommand.ItemGroup);
            model.Users.FirstOrDefault(x => x.Id == SecondUser.Id).Owed.Should().Be(-10);
            model.Id.Should().Be(project.Id);
            model.Created.Should().BeCloseTo(DateTime.Now, 1000);
            model.Users.FirstOrDefault(x => x.Id == User.Id).Should().NotBeNull();
            model.Title.Should().Be(project.Title);
            model.LastModified.Should().Be(null);
        }

        [Test]
        public void Handle_InvalidProjectId_ShouldThrowNotFoundException()
        {
         
            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = "asdasdas"
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<NotFoundException>();

        }

        [Test]
        public void Handle_EmptyProjectId_ShouldThrowValidationException()
        {
            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }

    }
}