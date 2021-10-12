using Syhler.InformationGathering.Application.Services.Interface;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Domain.Enums;

namespace Syhler.InformationGathering.Application.Services
{
    public class WebsiteTypeService : IWebsiteTypeService
    {

        private readonly string _music = "Music";
        
        public WebsiteInformationType GetTypeFromYoutube(YoutubeResultModel model)
        {
            if (model.Tags.Contains(_music))
            {
                return WebsiteInformationType.Music;
            }

            return WebsiteInformationType.Video;
        }
    }
}