using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Behaviours;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<CreateFinancialProjectCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;


        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<CreateFinancialProjectCommand>>();

            _currentUserService = new Mock<ICurrentUserService>();

            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task Handle_IsAuthenticated_ShouldCallGetUserNameAsyncOnce()
        {
            _currentUserService.Setup(x => x.UserID).Returns("Administrator");
            
            var requestLogger = new LoggingBehaviour<CreateFinancialProjectCommand>(_identityService.Object, _currentUserService.Object, _logger.Object, null);

            await requestLogger.Process(new CreateFinancialProjectCommand{Title = "Hey"}, new CancellationToken());
            
            _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Handle_IsNotAuthenticated_ShouldNotCallGetUserNameAsyncOnce()
        {
            var requestLogger = new LoggingBehaviour<CreateFinancialProjectCommand>(_identityService.Object, _currentUserService.Object, _logger.Object, null);

            await requestLogger.Process(new CreateFinancialProjectCommand{Title = "Hey"}, new CancellationToken());
            
            _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
        }
    }
}