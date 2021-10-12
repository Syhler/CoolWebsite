using System;
using System.Linq;
using Syhler.InformationGathering.Application.Services.Interface;

namespace Syhler.InformationGathering.Application.Services
{
    public class CurrentWebsiteService : ICurrentWebsiteService
    {
        public bool IsCurrentPageYoutubeMusic(string currentUrl, string[] urls)
        {
            var youtubeMusicUrl = "https://music.youtube.com/";
            
            var urlsContainsYoutube = urls.Contains(youtubeMusicUrl);

            if (!urlsContainsYoutube) throw new Exception("Youtube music isn't open");
            
            return currentUrl == youtubeMusicUrl;
        }
    }
}