using System;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Infrastructure
{
    
    //read up on IWebHostEnvironment for testing
    //read up on Assembly stuff
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddScoped<ApplicationDbContext>();
            
            //is instanced in startup in "coolwebsite" project
            
            // services.AddIdentityCore<ApplicationUser>()
            //     .AddEntityFrameworkStores<ApplicationDbContext>();
            

            return services;
        }
    }
}