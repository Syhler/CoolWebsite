using System;
using System.Collections.Generic;

namespace Syhler.InformationGathering.Application.Services.YoutubeApi
{
    public class YoutubeResultModel
    {
        public string Title { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string CategoryId { get; set; }
        public List<string> Tags { get; set; }
        public long ViewCount { get; set; }
        public long LikeCount { get; set; }
        public long DislikeCount { get; set; }
        public long CommentCount { get; set; }
        
    }
}