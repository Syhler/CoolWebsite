using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Domain.Enums;

namespace Syhler.InformationGathering.Application.Services.Interface
{
    public interface IWebsiteTypeService
    {
        WebsiteInformationType GetTypeFromYoutube(YoutubeResultModel model);
    }
}