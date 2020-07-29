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
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "First",
                Users = new List<ApplicationUser>
                {
                    User
                }
            };

            var project = await SendAsync(createCommand);


            var newlyCreatedUser = await CreateNewUser("test12312312312", "hahah");
            
            var updateCommand = new UpdateFinancialProjectCommand
            {
                Id = project.Id,
                Name = "Second",
                Users = new List<string>
                {
                    User.Id,
                    newlyCreatedUser.Id,
                    SecondUser.Id,
                    CreateNewUser("bla","bla123").Result.Id,
                    CreateNewUser("blaa", "nahhh").Result.Id
                },
                Description = "heyy"
            };

            await SendAsync(updateCommand);

            var context = CreateContext();

            var entity = context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include(x => x.OweRecords)
                .First(x => x.Id == project.Id);
            
            
            entity.Should().NotBeNull();
            entity.Title.Should().Be(updateCommand.Name);
            entity.FinancialProjectApplicationUsers.Count.Should().Be(5);
            entity.FinancialProjectApplicationUsers.FirstOrDefault(x => x.UserId == newlyCreatedUser.Id).Should().NotBeNull();
            entity.OweRecords.Count.Should().Be(20);
            entity.Description.Should().Be(updateCommand.Description);
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
        public async Task Handle_RemoveUser_ShouldUpdate()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "First",
                Users = new List<ApplicationUser>
                {
                    User,
                    SecondUser
                }
            };

            var project = await SendAsync(createCommand);
            
            var updateCommand = new UpdateFinancialProjectCommand
            {
                Id = project.Id,
                Name = "Second",
                Users = new List<string>
                {
                    User.Id,
                },
                Description = "nahhh"
            };

            await SendAsync(updateCommand);

            var context = CreateContext();

            var entity = context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include(x => x.OweRecords)
                .First(x => x.Id == project.Id);
            
            entity.Should().NotBeNull();
            entity.Title.Should().Be(updateCommand.Name);
            entity.FinancialProjectApplicationUsers.Count.Should().Be(1);
            entity.FinancialProjectApplicationUsers.FirstOrDefault(x => x.UserId == SecondUser.Id).Should().BeNull();
            entity.OweRecords.Count.Should().Be(0);
            entity.Description.Should().Be(updateCommand.Description);
            var recordFrom = entity.OweRecords.FirstOrDefault(x => x.UserId == SecondUser.Id);
            var recordTo = entity.OweRecords.FirstOrDefault(x => x.OwedUserId == SecondUser.Id);
            recordFrom.Should().BeNull();
            recordTo.Should().BeNull();
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
                Users = new List<string>
                {
                    SecondUser.Id
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
                Users = new List<string>
                {
                    SecondUser.Id
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
                Users = new List<string>
                {
                    SecondUser.Id
                }
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

       
        
        
        
    }
}