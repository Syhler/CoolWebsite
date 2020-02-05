using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolWebsite.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
            //serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());
          
            // var configuration = new MapperConfiguration(cfg =>
            // {
            //     cfg.AddProfile(new MappingProfile());
            // });
            //
            // //serviceCollection.AddSingleton<IMapper>(sp => configuration.CreateMapper());
            
            
            
            
            return serviceCollection;
        }
    }
}