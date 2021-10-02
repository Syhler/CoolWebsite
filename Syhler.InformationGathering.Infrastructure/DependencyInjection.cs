using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Syhler.InformationGathering.Application.Common.Interface;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Infrastructure.Api.Common;
using Syhler.InformationGathering.Infrastructure.Api.Youtube;
using Syhler.InformationGathering.Infrastructure.Repository;

namespace Syhler.InformationGathering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMemoryCache, MemoryCache>();
            serviceCollection.AddScoped<IMusicInformationRepository, MusicInformationRepository>();
            serviceCollection.AddScoped<IWebsiteInformationRepository, WebsiteInformationRepository>();
            serviceCollection.AddScoped<IYoutubeInformationRepository, YoutubeInformationRepository>();
            serviceCollection.AddScoped<IHttpClientFactory, HttpClientFactory>();
            serviceCollection.AddScoped<IYoutubeApiService, YoutubeApiService>();
            serviceCollection.AddScoped<ICacheService, CacheService>();
            return serviceCollection;
        }
    }
}