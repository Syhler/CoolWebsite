using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.CreateTestEntity;
using CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.UpdateTestEntity;
using CoolWebsite.UnitTest.Services;
using Shouldly;
using Xunit;

namespace CoolWebsite.UnitTest.TestEntities.UpdateTestEntity
{
    public class UpdateTestEntityCommandTest : CommandTestBase
    {

        [Fact]
        public async Task Handle_GivenValidId_ShouldUpdateName()
        {
            var createCommand = new CreateTestEntityCommand
            {
                Name = "First name"
            };
            
            var createHandler = new CreateTestEntityCommand.CreateTestEntityCommandHandler(Context, new CurrentUserService());

            var result = await createHandler.Handle(createCommand, CancellationToken.None);
            
            var updateCommand = new UpdateTestEntityCommand
            {
                Name = "new name",
                Id = result
            };
            
            var updateHandler = new UpdateTestEntityCommand.UpdateTestEntityCommandHandler(Context, new CurrentUserService());

            await updateHandler.Handle(updateCommand, CancellationToken.None);

            var entity = Context.TestEntities.Find(updateCommand.Id);
            
            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(updateCommand.Name);
        }

        [Fact]
        public void Handle_GivenInvalidId_ShouldThrowNotFoundException()
        {

            var command = new UpdateTestEntityCommand
            {
                Id = "ddd",
                Name = "bla"
            };

            var handler = new UpdateTestEntityCommand.UpdateTestEntityCommandHandler(Context, new CurrentUserService());
            
            
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
            
        }
    }
}