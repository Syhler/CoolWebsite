using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Syhler.InformationGathering.Application.Common.Interface;
using Syhler.InformationGathering.Application.Services.YoutubeApi;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Infrastructure.Api.Common;
using Syhler.InformationGathering.Infrastructure.Api.Youtube;
using Syhler.InformationGathering.Infrastructure.Context;
using Syhler.InformationGathering.Infrastructure.Repository;
using Syhler.InformationGathering.Infrastructure.Services;

namespace Syhler.InformationGathering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddScoped<IDateTimeService, DateTimeService>();

            var mysqlConnectionString = configuration.GetConnectionString("DefaultConnection");

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString));
            });
            
            serviceCollection.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            
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