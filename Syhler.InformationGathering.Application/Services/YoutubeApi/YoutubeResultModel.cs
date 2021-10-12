using System;
using System.Collections.Generic;

namespace Syhler.InformationGathering.Application.Services.YoutubeApi
{
    public class YoutubeResultModel
    {
        public string Title { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string ChannelTitle { get; set; } = null!;
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public int CategoryId { get; set; }
        public List<string> Tags { get; set; } = null!;
        public long ViewCount { get; set; }
        public long LikeCount { get; set; }
        public long DislikeCount { get; set; }
        public long CommentCount { get; set; }
        
    }
}