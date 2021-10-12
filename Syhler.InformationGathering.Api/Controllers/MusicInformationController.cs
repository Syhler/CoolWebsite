using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Syhler.InformationGathering.Api.Request;
using Syhler.InformationGathering.Application.Services.Interface;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateMusicInformation;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateWebsiteInformation;

namespace Syhler.InformationGathering.Api.Controllers
{
    [ApiController]
    [Route("api/music-information")]
    public class MusicInformationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentWebsiteService _currentWebsiteService;


        public MusicInformationController(IMediator mediator, ICurrentWebsiteService currentWebsiteService)
        {
            _mediator = mediator;
            _currentWebsiteService = currentWebsiteService;
        }

        [HttpPost("youtube")]
        public async Task<IActionResult> PostYoutubeMusic([FromBody] MusicInformationPostRequest request)
        {
            Console.WriteLine("MUSIC INFORMATION");
            
            if (!request.IsValid()) return BadRequest();
            
            //var isCurrentPage = _currentWebsiteService.IsCurrentPageYoutubeMusic(request.CurrentUrl, request.Urls);

            var command = new CreateMusicInformationCommand(
                request.CurrentUrl,
                request.TimeVisited,
                request.IsInFocus, 
                true, 
                request.SongInfo!.Title, 
                request.SongInfo!.Artist);

            await _mediator.Send(command);

            var urlsNotCurrentPage = request.Urls.Where(x => !x.Equals(request.CurrentUrl));

            var tabCommand = new CreateTabWebsiteInformationCommand(urlsNotCurrentPage, request.TimeVisited);

            await _mediator.Send(tabCommand);
            
            

            return Ok();
        }
    }
}