using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.UpdateFinancialProject;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ValidationException = CoolWebsite.Application.Common.Exceptions.ValidationException;

namespace Application.IntegrationTests.Financial.FinancialProject.Commands
{
    using static Testing;
    public class UpdateFinancialProjectTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_ValidFields_ShouldUpdate()
        {


            var firstUser = await CreateNewUser("CoolUser", "Not cool");
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "First",
                Users = new List<ApplicationUser>
                {
                    User,
                    firstUser
                }
            };

            var project = await SendAsync(createCommand);


            var newlyCreatedUser = await CreateNewUser("test12312312312", "hahah");
            
            var updateCommand = new UpdateFinancialProjectCommand
            {
                Id = project.Id,
                Name = "Second",
                Users = new List<ApplicationUser>
                {
                    newlyCreatedUser
                }
            };

            await SendAsync(updateCommand);

            var context = CreateContext();

            var entity = context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include(x => x.OweRecords)
                .First(x => x.Id == project.Id);
            
            
            entity.Should().NotBeNull();
            entity.Title.Should().Be(updateCommand.Name);
            entity.FinancialProjectApplicationUsers.FirstOrDefault(x => x.UserId == newlyCreatedUser.Id).Should().NotBeNull();
            var recordFrom = entity.OweRecords.FirstOrDefault(x => x.UserId == newlyCreatedUser.Id);
            var recordTo = entity.OweRecords.FirstOrDefault(x => x.OwedUserId == newlyCreatedUser.Id);
            recordFrom.Should().NotBeNull();
            recordTo.Should().NotBeNull();
            entity.LastModified.Should().NotBeNull();
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 10000);
            entity.LastModifiedBy.Should().NotBeNull();
            entity.LastModifiedBy.Should().Be(User.Id);
        }
        

        [Test]
        public async Task Handle_EmptyTitle_ShouldThrowValidationException()
        {
            var id = await CreateFinancialProject();
            
            var command = new UpdateFinancialProjectCommand
            {
                Name = "",
                Id = id,
                Users = new List<ApplicationUser>
                {
                    await RunAsDefaultUserAsync()
                }
            };

            FluentActions.Invoking(async () =>await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_OverMaxTitleLength_ShouldThrowValidationException()
        {
            var id = await CreateFinancialProject();
            
            var command = new UpdateFinancialProjectCommand()
            {
                Name = "asdflhsdofghsodufjhsdoijfsdoijfiosdjfiodsjoifjdfgdfgdfdfgdfgækdfjgkædkfælgfdkogådfkopgkfdpogopdfkgpodfkpgkpogdfgdosdfoijsdoifjosdjofisd",
                Users = new List<ApplicationUser>
                {
                    await RunAsDefaultUserAsync()
                },
                Id = id
            };
            FluentActions.Invoking(async () =>await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_InvalidFields_ShouldThrowValidationException()
        {
            var id = await CreateFinancialProject();
            
            var command = new UpdateFinancialProjectCommand();

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new UpdateFinancialProjectCommand
            {
                Id = "asdasdsa",
                Name = "hey",
                Users = new List<ApplicationUser>
                {
                    await RunAsDefaultUserAsync()
                }
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

       
        
        
        
    }
}