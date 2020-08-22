using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts;
using CoolWebsite.Areas.Financial.Models;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Commands
{
    using static Testing;
    
    public class UpdateReceiptsTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidFields_ShouldUpdate()
        {
            var newProject = await CreateFinancialProject();
            var projectId = await CreateFinancialProject();
            var id = await CreateReceipt(projectId);

            var command = new UpdateReceiptCommand
            {
                Id = id,
                Location = "Netto",
                DateVisited = DateTime.Now,
                Note = "meh",
                FinancialProjectId = newProject
            };

            
            await SendAsync(command);

            var context = CreateContext();
            var entity = context.Receipts.FirstOrDefault(x => x.Id == command.Id);

            entity.Should().NotBeNull();
            entity.Note.Should().Be(command.Note);
            entity.FinancialProjectId.Should().Be(newProject);
            entity.Location.Should().Be(command.Location);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(User.Id);
        }

        [Test]
        public async Task Handle_WithoutFinancialProjectId_ShouldUpdate()
        {
            var projectId = await CreateFinancialProject();
            var id = await CreateReceipt(projectId);
            
            var command = new UpdateReceiptCommand
            {
                Id = id,
                Location = "Title",
                DateVisited = DateTime.Now,
            };

            await SendAsync(command);

            var entity = await FindAsync<Receipt>(command.Id);

            entity.Should().NotBeNull();
            entity.FinancialProjectId.Should().Be(projectId);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(User.Id);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            entity.Location.Should().Be(command.Location);
        }

        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "nah",
                Location = "Title",
                DateVisited = DateTime.Now,
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "",
                Location = "Title",
                DateVisited = DateTime.Now
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        
        [Test]
        public void Handle_BoughtAtEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "asdadas",
                Location = "Title",
            };
            
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test]
        public void Handle_TitleEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "asdadas",
                DateVisited = DateTime.Now,
                Location = ""
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_TitleAboveMaxLength_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "asdadas",
                Location = "dafhdsugjhsfdosjdfjiodsfoijdsfjiosdfsdfdfsdfsdfsdfsfsdfsdfsdf" +
                        "dfgdfgdfgojifdsjoifsdjoisdfojisdfjoisdfjoifsojdijoisdfjoifsdoij",
                DateVisited = DateTime.Now
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
        
    }
}