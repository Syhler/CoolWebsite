using System;

namespace Syhler.InformationGathering.Api.Request
{
    public class MusicInformationPostRequest
    {
        public string CurrentUrl { get; set; } = null!;
        public bool IsInFocus { get; set; }
        public string[] Urls { get; set; } = null!;
        public SongInformation? SongInfo { get; set; }

        public DateTime TimeVisited { get; set; }

        public bool IsValid()
        {
            return SongInfo.ValidationCheck();
        }
        
        public class SongInformation
        {
            public string? Artist { get; set; }
            public string? Title { get; set; }

            public bool ValidationCheck()
            {
                if (string.IsNullOrWhiteSpace(Artist)) return false;
                if (string.IsNullOrWhiteSpace(Title)) return false;

                return true;
            }
        }
    }
    
    
}