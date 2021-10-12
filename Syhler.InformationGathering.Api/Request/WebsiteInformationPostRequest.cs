using System;
using System.Collections.Generic;

namespace Syhler.InformationGathering.Api.Request
{
    public class WebsiteInformationPostRequest
    {
        public List<string> Urls { get; set; } = null!;
        public string CurrentUrl { get; set; } = null!;
        public DateTime TimeVisited { get; set; }
        public bool IsInFocus { get; set; }
    }

    public class YoutubeInformationPostRequest
    {
        public string CurrentUrl { get; set; } = null!;
        public DateTime TimeVisited { get; set; }
        public bool IsInFocus { get; set; }
        public string VideoId { get; set; } = null!;

    }
}