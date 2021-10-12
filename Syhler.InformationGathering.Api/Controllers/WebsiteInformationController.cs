using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Syhler.InformationGathering.Api.Request;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateWebsiteInformation;

namespace Syhler.InformationGathering.Api.Controllers
{
    [ApiController]
    [Route("api/website-information")]
    public class WebsiteInformationController : ControllerBase
    {

        private readonly IMediator _mediator;

        public WebsiteInformationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(WebsiteInformationPostRequest request)
        {

            Console.WriteLine("WEBSITE INFORMATION");
            
            var command = new CreateWebsiteInformationCommand(
                request.CurrentUrl,
                request.TimeVisited,
                request.IsInFocus,
                true);

            await _mediator.Send(command);
            
            var urlsNotCurrentPage = request.Urls.Where(x => !x.Equals(request.CurrentUrl));

            var tabCommand = new CreateTabWebsiteInformationCommand(urlsNotCurrentPage, request.TimeVisited);

            await _mediator.Send(tabCommand);
            
            return Ok();
        }
    }
}