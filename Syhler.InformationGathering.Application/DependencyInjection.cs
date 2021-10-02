using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Syhler.InformationGathering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());

            
            
            return serviceCollection;
        }
    }
}