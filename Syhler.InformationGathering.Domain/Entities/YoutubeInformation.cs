using System;
using System.Data.Common;
using Syhler.InformationGathering.Domain.Enums;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Domain.Entities
{
    public class YoutubeInformation : WebsiteInformation
    {
        //public bool IsMusic { get; set; }
        public string Category { get; } 
        public bool IsPlayingAndNotFocus { get; }
        public bool IsPlayingAndNotFocusNorCurrentPage { get; }
        public string Title { get; }
        public string UrlId { get; }
        public string ChannelName { get; }

        public YoutubeInformation(WebsiteId id, string url, bool isCurrentPage, bool isInFocus, WebsiteInformationType type,
            DateTime timeVisited,
            string category, string title, string urlId, string channelName, bool isPlayingAndNotFocusNorCurrentPage, 
            bool isPlayingAndNotFocus) 
            : base(id, url, isCurrentPage, isInFocus, type, timeVisited)
        {
            Category = category;
            Title = title;
            UrlId = urlId;
            ChannelName = channelName;
            IsPlayingAndNotFocusNorCurrentPage = isPlayingAndNotFocusNorCurrentPage;
            IsPlayingAndNotFocus = isPlayingAndNotFocus;
        }
    }
}