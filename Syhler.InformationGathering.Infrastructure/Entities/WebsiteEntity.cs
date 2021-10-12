using System;
using System.Collections.Generic;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Infrastructure.Entities
{
    public class WebsiteEntity
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsCurrentPage { get; set; }
        public bool IsInFocus { get; set; }
        
        public DateTime TimeVisited { get; set; }

        public ICollection<MusicEntity> MusicEntities { get; set; } = new List<MusicEntity>();
        public ICollection<YoutubeEntity> YoutubeEntities { get; set; } = new List<YoutubeEntity>();


        public static WebsiteEntity FromYoutubeDomain(YoutubeInformation model)
        {
            return new WebsiteEntity
            {
                Url = model.Url,
                IsCurrentPage = model.IsCurrentPage,
                IsInFocus = model.IsInFocus,
                TimeVisited = model.TimeVisited
            };
        }

        public static WebsiteEntity FromMusicDomain(MusicInformation model)
        {
            return new WebsiteEntity
            {
                Url = model.Url,
                IsCurrentPage = model.IsCurrentPage,
                IsInFocus = model.IsInFocus,
                TimeVisited = model.TimeVisited
            };
        }

        public static WebsiteEntity FromWebsiteDomain(WebsiteInformation model)
        {
            return new WebsiteEntity
            {
                Url = model.Url,
                IsCurrentPage = model.IsCurrentPage,
                IsInFocus = model.IsInFocus,
                TimeVisited = model.TimeVisited
            };
        }
    }
}