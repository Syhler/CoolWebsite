using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoolWebsite.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        private string _ip;
        private string _userAgent;

        public LoggingBehaviour(IIdentityService identityService, ICurrentUserService currentUserService,
            ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
            _logger = logger;
            _ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            _userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserID ?? string.Empty;
            string userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }
            
            _logger.LogInformation("CoolWebsite Request : {Name} {@UserId} {@UserName} {@Request} {@UserAgent} {@Ip}",
                requestName, userId, userName, request, _userAgent, _ip);
        }
    }
}