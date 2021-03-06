using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoolWebsite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<MySqlApplicationDbContext>();

                    await context.Database.MigrateAsync();

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    
                    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);

                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    
                    logger.LogError(e,"An error occured while migrating or seeding the database");
                    
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel()
                        .UseStartup<Startup>();
                });
            
            
            return host;
        }
          
    }
}