using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Syhler.InformationGathering.Application.Services;
using Syhler.InformationGathering.Application.Services.Interface;

namespace Syhler.InformationGathering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());

            serviceCollection.AddScoped<IWebsiteTypeService, WebsiteTypeService>();
            serviceCollection.AddScoped<ICurrentWebsiteService, CurrentWebsiteService>();
            
            
            return serviceCollection;
        }
    }
}