using System.Threading.Tasks;
using CoolWebsite.Application.Common.Models;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userID);

        Task<Result> CreateUserAsync(string email, string password);
        

    }
}