using System.Threading.Tasks;

namespace Syhler.InformationGathering.Application.Common.Interface
{
    public interface ICacheService
    {
        Task<bool> Save<T>(string key, T value);
        Task<bool> Save<T>(string key, T value, int duration);
        
        
        Task<T> Get<T>(string key);
        Task<bool> TryAndGet<T>(string key, out T value);

    }
}