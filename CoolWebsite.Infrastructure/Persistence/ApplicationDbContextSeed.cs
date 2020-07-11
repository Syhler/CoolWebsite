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
            
            var secondUser = new ApplicationUser
            {
                UserName = "Lucas.ajander@gmail.com",
                Email = "Lucas.ajander@gmail.com",
                FirstName = "Lucas",
                LastName = "Ajander"
            };
            var thirdUser = new ApplicationUser
            {
                UserName = "Niklas.hesteegg@gmail.com",
                Email = "Niklas.hesteegg@gmail.com",
                FirstName = "Niklas",
                LastName = "Hesteegg"
            };
            
            var guestUser = new ApplicationUser
            {
                FirstName = "Guest",
                LastName = "Guest",
                UserName = "Guest123@guest.com",
                Email = "Guest123@guest.com"
            };

            if (roleManager.Roles.All(u => u.Name != "Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole {Name = "Admin"});
            }
            
            if (roleManager.Roles.All(u => u.Name != "Financial"))
            {
                await roleManager.CreateAsync(new ApplicationRole {Name = "Financial"});
            }

            await CreateUser(userManager, defaultUser);
            await CreateUser(userManager, secondUser);
            await CreateUser(userManager, thirdUser);
            await CreateUser(userManager, guestUser, "Guest123");

            await GiveRole(userManager, defaultUser.Email, "Admin");
            await GiveRole(userManager, secondUser.Email, "Financial");
            await GiveRole(userManager, thirdUser.Email, "Financial");
            await GiveRole(userManager, guestUser.Email, "Financial");

        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, "testUser123");
            }
        }
        
        private static async Task CreateUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
        {
            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, password);
            }
        }

        private static async  Task GiveRole( UserManager<ApplicationUser> userManager, string email, string role)
        {
            var user = userManager.Users.First(x => x.Email == email);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}