using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateYoutubeInformation;

namespace Syhler.InformationGathering.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IYoutubeApiService _youtubeApiService;
        private readonly IMediator _mediator;
        
        
        public TestController(IYoutubeApiService youtubeApiService, IMediator mediator)
        {
            _youtubeApiService = youtubeApiService;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {

            var result = await _mediator.Send(new CreateYoutubeInformationCommand
            {
                DateTime = DateTime.Now,
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