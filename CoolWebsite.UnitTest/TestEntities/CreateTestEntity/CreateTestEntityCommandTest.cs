using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity;
using CoolWebsite.UnitTest.Services;
using Shouldly;
using Xunit;

namespace CoolWebsite.UnitTest.TestEntities.CreateTestEntity
{
    public class CreateTestEntityCommandTest : CommandTestBase
    {
        
        [Fact]
        public async Task Handle_ShouldPersistTestEntity()
        {
            var command = new CreateTestEntityCommand
            {
                Name = "new test"
            };
            
            var handler = new CreateTestEntityCommand.CreateTestEntityCommandHandler(Context, new CurrentUserServiceTemp());

            var result = await handler.Handle(command, CancellationToken.None);

            var entity = Context.TestEntities.Find(result);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);

        }
    }
}