using System.Linq;
using System.Threading.Tasks;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "morten.syhler@gmail.com",
                Email = "morten.syhler@gmail.com",
                FirstName = "Morten",
                LastName = "Syhler"
            };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "testUser123");
                
                if (roleManager.Roles.All(u => u.Name != "Admin"))
                {
                    await roleManager.CreateAsync(new ApplicationRole {Name = "Admin"});
                }
                
                var user = userManager.Users.First(x => x.Email == defaultUser.Email);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}