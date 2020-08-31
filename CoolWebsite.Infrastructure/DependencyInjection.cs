using System;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Infrastructure.Identity;
using CoolWebsite.Infrastructure.Persistence;
using CoolWebsite.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

            SetupDatabase<SqlApplicationDbContext>(services, configuration);
           
            
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Index";
                options.LogoutPath = "/Home/Index";
            });
 
            return services;
        }


        private static void SetupDatabase<T>(IServiceCollection services, IConfiguration configuration) where T : DbContext, IApplicationDbContext
        {
            if (typeof(T) == typeof(MySqlApplicationDbContext))
            {
                services.AddDbContext<T>(builder =>
                {
                    builder.UseMySql(configuration.GetConnectionString("DefaultConnectionMySQL"));
                });
            }
            else if (typeof(T) == typeof(SqlApplicationDbContext))
            {
                services.AddDbContext<T>(builder =>
                {
                    builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
            }
            
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<T>());

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<T>()
                .AddDefaultTokenProviders();
        }
    }
}