namespace Syhler.InformationGathering.Application.Services.Interface
{
    public interface ICurrentWebsiteService
    {
        bool IsCurrentPageYoutubeMusic(string currentUrl, string[] urls);
    }
}