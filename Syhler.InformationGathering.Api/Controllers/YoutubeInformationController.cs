using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateYoutubeInformation;

namespace Syhler.InformationGathering.Api.Controllers
{
    [ApiController]
    [Route("api/youtube-information")]
    public class YoutubeInformationController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        
        public YoutubeInformationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {

            var result = await _mediator.Send(new CreateYoutubeInformationCommand
            {
                TimeVisited = DateTime.Now,
                IsCurrentPage = false,
                IsInFocus = false,
                IsPlayingAndNotFocus = true,
                IsPlayingAndNotFocusNorCurrentPage = true,
                Url = id
            });

            return Ok(result);
        }
        
        

      
    }
}