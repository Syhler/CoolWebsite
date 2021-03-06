﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using Respawn;

namespace Application.IntegrationTests
{
    
    
    [SetUpFixture]
    public class Testing
    {
       
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;
        public static string CurrentUserId;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",true,true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();


            var webHostEnvironment = Mock.Of<IWebHostEnvironment>(w => w.EnvironmentName == "Development" &&
                                                                      w.ApplicationName == "CoolWebsite");
            
            var startup = new Startup(_configuration,webHostEnvironment);
            
            var services = new ServiceCollection();


            services.AddSingleton(webHostEnvironment);
            services.AddLogging();
            
            startup.ConfigureServices(services);
            
            //Removes already existing current user service
            var currentUserServicesDescriptor =
                services.FirstOrDefault(x => x.ServiceType == typeof(ICurrentUserService));

            services.Remove(currentUserServicesDescriptor);

            services.AddTransient(x => Mock.Of<ICurrentUserService>(s => s.UserId == CurrentUserId));

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new []{"__EFMigrationsHistory"},
            };
            
            
            
            EnsureDatabase();
        }
        
        public static async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }


        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlApplicationDbContext>();
            
            context.Database.Migrate();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        public static async Task<ApplicationUser> CreateNewUser(string username, string password)
        {
            return await CreateUser(username, password);
        }
        
        public static async Task<ApplicationUser> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local","Testing1234!");
        }

        public static async Task<ApplicationUser> RunAsUserAsync(string username, string password)
        {

            var user = await CreateUser(username, password);

            CurrentUserId = user.Id;

            return user;
        }

        private static async Task<ApplicationUser> CreateUser(string username, string password)
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            
            var user = new ApplicationUser{UserName = username, Email = username, FirstName = username, LastName = password};

            await userManager.CreateAsync(user, password);

            return user;
        }
       

        public static async Task<TEntity> FindAsync<TEntity>(string id) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlApplicationDbContext>();

            
            return await context.FindAsync<TEntity>(id);
            
        }

        public static SqlApplicationDbContext CreateContext()
        {
            var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<SqlApplicationDbContext>();
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlApplicationDbContext>();

            await context.AddAsync(entity);

            await context.SaveChangesAsync(new CancellationToken());
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            
        }
    }
}