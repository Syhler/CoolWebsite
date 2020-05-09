using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Commands
{
    using static Testing;

    public class CreateFinancialProjectTests : TestBase
    {
        [Test]
        public async Task Handle_EmptyTitle_ShouldThrowValidationException()
        {
           var command = new CreateFinancialProjectCommand
           {
               Name = "",
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
                Name = "asdaohsfgdouhdsfgouhifgdoijgfdojifdgoijfgdjoifdgojifgdjoigdfjoiogjfdiojigdfjfodgoidfjogidfjoigjiodfjgodfoigjodfigiodfiogjdiofgiodfgjdfo",
                Users = new List<ApplicationUser>
                {
                    await RunAsDefaultUserAsync()
                }
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_InvalidFields_ShouldThrowValidationException()
        {
            var command = new CreateFinancialProjectCommand();
            
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_Create_ShouldCreateFinancialProject()
        {
            var user = await RunAsDefaultUserAsync();

            await AddAsync(user);
            
            var command = new CreateFinancialProjectCommand
            {
                Name = "Wooow",
                Users = new List<ApplicationUser>
                {
                    user
                }
            };

            var id = await SendAsync(command);

            var context = Context();
            
            var entity = context.FinancialProjects.Include(x => x.FinancialProjectApplicationUsers)
                .FirstOrDefault(x => x.Id == id);
            
            await context.DisposeAsync();
            

            entity.Should().NotBeNull();
            entity.Name.Should().Be(command.Name);
            entity.FinancialProjectApplicationUsers.First().UserId.Should().Be(user.Id);
            entity.CreatedBy.Should().Be(user.Id);
            entity.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}