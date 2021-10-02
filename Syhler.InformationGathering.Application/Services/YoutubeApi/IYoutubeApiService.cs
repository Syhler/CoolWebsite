using System.Threading.Tasks;

namespace Syhler.InformationGathering.Application.Services.YoutubeApi
{
    public interface IYoutubeApiService
    {
        public Task<YoutubeResultModel> RequestInformation(string videoId);
    }
}