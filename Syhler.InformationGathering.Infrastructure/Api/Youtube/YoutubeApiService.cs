using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.Common.Interface;
using Syhler.InformationGathering.Application.Services.YoutubeApi;

namespace Syhler.InformationGathering.Infrastructure.Api.Youtube
{
    public class YoutubeApiService : IYoutubeApiService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public YoutubeApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<YoutubeResultModel> RequestInformation(string videoId)
        {
            var key = "AIzaSyDhL1c3Rg2fob7YdZt1-Ood_bWZrnmzAtU";

            var client = _httpClientFactory.InstantiateHttpClient();

            var responseMessage = await client.GetAsync(CreateUrl(videoId, key));

            var youtubeResponse = await responseMessage.Content.ReadFromJsonAsync<YoutubeResponse>();

            if (youtubeResponse == null) throw new Exception("idk"); //TODO CHANGE TO A PROPER EXCEPTION

            var youtubeItem = youtubeResponse.Items.First();
            var youtubeSnippet = youtubeItem.Snippet;
            
            return new YoutubeResultModel
            {
                Title = youtubeSnippet.Title,
                ChannelId = youtubeSnippet.ChannelId,
                ChannelTitle = youtubeSnippet.ChannelTitle,
                PublishedAt = youtubeSnippet.PublishedAt,
                Description = youtubeSnippet.Description,
                ThumbnailUrl = youtubeSnippet.Thumbnails.High.Url,
                CategoryId = Convert.ToInt32(youtubeSnippet.CategoryId),
                Tags = youtubeSnippet.Tags,
                ViewCount = Convert.ToInt64(youtubeItem.Statistics.ViewCount),
                LikeCount = Convert.ToInt64(youtubeItem.Statistics.LikeCount),
                DislikeCount = Convert.ToInt64(youtubeItem.Statistics.DislikeCount),
                CommentCount = Convert.ToInt64(youtubeItem.Statistics.CommentCount),
            };

        }

        private string CreateUrl(string videoId, string key)
        {
            return
                $"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={key}&part=snippet,contentDetails,statistics,status";
        }
        
        
 
   private  class Default
   {
       public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Medium
    {
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class High
    {
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Standard
    {
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Maxres
    {
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Thumbnails
    {
        public Default Default { get; set; } = null!;
        public Medium Medium { get; set; } = null!;
        public High High { get; set; } = null!;
        public Standard Standard { get; set; } = null!;
        public Maxres Maxres { get; set; } = null!;
    } 

    private  class Localized
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    private  class Snippet
    {
        public DateTime PublishedAt { get; set; }
        public string ChannelId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Thumbnails Thumbnails { get; set; } = null!;
        public string ChannelTitle { get; set; } = null!;
        public List<string> Tags { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public string LiveBroadcastContent { get; set; } = null!;
        public Localized Localized { get; set; } = null!;
        public string DefaultAudioLanguage { get; set; } = null!;
    }

    private  class ContentRating
    {
    }

    private  class ContentDetails
    {
        public string Duration { get; set; } = null!;
        public string Dimension { get; set; } = null!;
        public string Definition { get; set; } = null!;
        public string Caption { get; set; } = null!;
        public bool LicensedContent { get; set; }
        public ContentRating ContentRating { get; set; } = null!;
        public string Projection { get; set; } = null!;
    }

    private  class Status
    {
        public string UploadStatus { get; set; } = null!;
        public string PrivacyStatus { get; set; } = null!;
        public string License { get; set; } = null!;
        public bool Embeddable { get; set; }
        public bool PublicStatsViewable { get; set; }
        public bool MadeForKids { get; set; }
    }

    private  class Statistics
    {
        public string ViewCount { get; set; } = null!;
        public string LikeCount { get; set; } = null!;
        public string DislikeCount { get; set; } = null!;
        public string FavoriteCount { get; set; } = null!;
        public string CommentCount { get; set; } = null!;
    }

    private  class Item
    {
        public string Kind { get; set; } = null!;
        public string Etag { get; set; } = null!;
        public string Id { get; set; } = null!;
        public Snippet Snippet { get; set; } = null!;
        public ContentDetails ContentDetails { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public Statistics Statistics { get; set; } = null!;
    }

    private  class PageInfo
    {
        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; }
    }

    private class YoutubeResponse
    {
        public string Kind { get; set; } = null!;
        public string Etag { get; set; } = null!;
        public List<Item> Items { get; set; } = null!;
        public PageInfo PageInfo { get; set; } = null!;
    }




    }
}