using Microsoft.Extensions.DependencyInjection;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Infrastructure.Repository;

namespace Syhler.InformationGathering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMusicInformationRepository, MusicInformationRepository>();
            serviceCollection.AddScoped<IWebsiteInformationRepository, WebsiteInformationRepository>();
            serviceCollection.AddScoped<IYoutubeInformationRepository, YoutubeInformationRepository>();
            return serviceCollection;
        }
    }
}