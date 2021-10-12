using Microsoft.EntityFrameworkCore.ChangeTracking;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Infrastructure.Entities
{
    public class YoutubeEntity
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public bool IsPlayingAndNotFocus { get; set; }
        public bool IsPlayingAndNotFocusNorCurrentPage { get; set; }
        public string Title { get; set; } = null!;
        //public string UrlId { get; set; } = null!;
        public string ChannelName { get; set; } = null!;
        
        public int WebsiteEntityId { get; set; }
        public WebsiteEntity WebsiteEntity { get; set; } = null!;

        public static YoutubeEntity FromDomain(YoutubeInformation model)
        {
            return new YoutubeEntity
            {
                Category = model.Category,
                IsPlayingAndNotFocus = model.IsPlayingAndNotFocus,
                IsPlayingAndNotFocusNorCurrentPage = model.IsPlayingAndNotFocusNorCurrentPage,
                ChannelName = model.ChannelName,
                Title = model.Title,
            };
        }
    }
}