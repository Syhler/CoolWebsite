using System;
using Syhler.InformationGathering.Domain.Enums;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Domain.Entities
{
    public class MusicInformation : WebsiteInformation
    {
        public string Title { get; init; }
        public string Artist { get; init; }
        public string? Genre { get; init; }

        public MusicInformation(WebsiteId id, string url, bool isCurrentPage, bool isInFocus, WebsiteInformationType type,
            DateTime timeVisited,
            string title, string artist) 
            : base(id, url, isCurrentPage, isInFocus, type, timeVisited)

        {
            Title = title;
            Artist = artist;
        }
    }
}