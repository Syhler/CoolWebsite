using System;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace CoolWebsite.UnitTest
{
    public class ApplicationDbFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            
            // Sets default datetime when testing time
            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock.Setup(m => m.Now)
                .Returns(new DateTime(3001, 1, 1));
            
            // Sets default userid when testing something with the user
            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.Setup(x => x.UserID)
                .Returns(Guid.Empty.ToString);
            
            var context = new ApplicationDbContext(options, dateTimeMock.Object);
            context.UserId = currentUserService.Object.UserID; //Because of the funcky implementation of currentUserService. shall be removed in the furture

            context.Database.EnsureCreated();

            SeedSampleData(context);
            
            
            return context;
        }

        private static void SeedSampleData(ApplicationDbContext context)
        {

            context.SaveChanges();
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}