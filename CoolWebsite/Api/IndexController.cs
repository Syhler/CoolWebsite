using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syhler.InformationGathering.Application.Services.YoutubeApi;

namespace CoolWebsite.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndexController : ControllerBase
    {
        private readonly IYoutubeApiService _youtubeApiService;

        public IndexController(IYoutubeApiService youtubeApiService)
        {
            _youtubeApiService = youtubeApiService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {


            var result = await _youtubeApiService.RequestInformation(id);

            return Ok(result);
        }
    }
}