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
                CategoryId = youtubeSnippet.CategoryId,
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
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Medium
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class High
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Standard
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Maxres
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    private  class Thumbnails
    {
        public Default Default { get; set; }
        public Medium Medium { get; set; }
        public High High { get; set; }
        public Standard Standard { get; set; }
        public Maxres Maxres { get; set; }
    }

    private  class Localized
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    private  class Snippet
    {
        public DateTime PublishedAt { get; set; }
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Thumbnails Thumbnails { get; set; }
        public string ChannelTitle { get; set; }
        public List<string> Tags { get; set; }
        public string CategoryId { get; set; }
        public string LiveBroadcastContent { get; set; }
        public Localized Localized { get; set; }
        public string DefaultAudioLanguage { get; set; }
    }

    private  class ContentRating
    {
    }

    private  class ContentDetails
    {
        public string Duration { get; set; }
        public string Dimension { get; set; }
        public string Definition { get; set; }
        public string Caption { get; set; }
        public bool LicensedContent { get; set; }
        public ContentRating ContentRating { get; set; }
        public string Projection { get; set; }
    }

    private  class Status
    {
        public string UploadStatus { get; set; }
        public string PrivacyStatus { get; set; }
        public string License { get; set; }
        public bool Embeddable { get; set; }
        public bool PublicStatsViewable { get; set; }
        public bool MadeForKids { get; set; }
    }

    private  class Statistics
    {
        public string ViewCount { get; set; }
        public string LikeCount { get; set; }
        public string DislikeCount { get; set; }
        public string FavoriteCount { get; set; }
        public string CommentCount { get; set; }
    }

    private  class Item
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string Id { get; set; }
        public Snippet Snippet { get; set; }
        public ContentDetails ContentDetails { get; set; }
        public Status Status { get; set; }
        public Statistics Statistics { get; set; }
    }

    private  class PageInfo
    {
        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; }
    }

    private class YoutubeResponse
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public List<Item> Items { get; set; }
        public PageInfo PageInfo { get; set; }
    }




    }
}