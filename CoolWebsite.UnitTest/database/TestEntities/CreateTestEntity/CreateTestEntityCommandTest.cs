using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity;
using CoolWebsite.UnitTest.Common;
using CoolWebsite.UnitTest.Services;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace CoolWebsite.UnitTest.TestEntities.CreateTestEntity
{
    using static Testing;

    public class CreateTestEntityCommandTest : CommandTestBase
    {

        [Fact]
        public async Task Handle_ShouldPersistTestEntity()
        {
            var command = new CreateTestEntityCommand
            {
                Name = "new test"
            };
            
            var handler = new CreateTestEntityCommand.CreateTestEntityCommandHandler(Context, new CurrentUserService());

            var result = await handler.Handle(command, CancellationToken.None);

            var entity = Context.TestEntities.Find(result);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
            

        }

        /*
        [Fact]
        public async Task Validator_ShouldNotAllow()
        { 
            
            var mediator = new  Mock<IMediator>();
            
            var command = new CreateTestEntityCommand();

            var handler = new CreateTestEntityCommand.CreateTestEntityCommandHandler(Context, new CurrentUserServiceTemp());
           
            Should.Throw<ValidationException>(() =>
            {
                handler.Handle(command, CancellationToken.None);
            });
            
        }
        */
    }
}