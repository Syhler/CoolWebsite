using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Commands
{
    using static Testing;

    public class CreateFinancialProjectTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_EmptyTitle_ShouldThrowValidationException()
        {
           var command = new CreateFinancialProjectCommand
           {
               Title = "",
               Users = new List<ApplicationUser>
               {
                   await RunAsDefaultUserAsync()
               }
           };

           FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_OverMaxTitleLength_ShouldThrowValidationException()
        {
            var command = new CreateFinancialProjectCommand
            {
                Title = "asdaohsfgdouhdsfgouhifgdoijgfdojifdgoijfgdjoifdgojifgdjoigdfjoiogjfdiojigdfjfodgoidfjogidfjoigjiodfjgodfoigjodfigiodfiogjdiofgiodfgjdfo",
                Users = new List<ApplicationUser>
                {
                    await RunAsDefaultUserAsync()
                }
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_InvalidFields_ShouldThrowValidationException()
        {
            var command = new CreateFinancialProjectCommand();
            
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_Create_ShouldCreateFinancialProject()
        {

            var command = new CreateFinancialProjectCommand
            {
                Title = "Wooow",
                Users = new List<ApplicationUser>
                {
                    User,
                    SecondUser
                }
            };

            var project = await SendAsync(command);

            var context = CreateContext();

            var entity = context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include(x => x.OweRecords)
                .FirstOrDefault(x => x.Id == project.Id);

            var oweRecords = context.OweRecords.Where(x => x.FinancialProjectId == project.Id);

            oweRecords.Should().NotBeNull();
            oweRecords.Count().Should().Be(2);
            
            await context.DisposeAsync();
            

            entity.Should().NotBeNull();
            entity.Title.Should().Be(command.Title);
            entity.FinancialProjectApplicationUsers.FirstOrDefault(x => x.UserId == User.Id).Should().NotBeNull();
            var record = entity.OweRecords.FirstOrDefault(x => x.UserId == User.Id);
            record.Should().NotBeNull();
            record.Amount.Should().Be(0);
            record.FinancialProjectId.Should().Be(project.Id);
            entity.CreatedBy.Should().Be(User.Id);
            entity.Created.Should().BeCloseTo(DateTime.Now, 10000);
            entity.FinancialProjectApplicationUsers.First().FinancialProjectId.Should().Be(project.Id);
        }
    }
}