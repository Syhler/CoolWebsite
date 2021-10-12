using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Syhler.InformationGathering.Application.Common.Interface;
using MemoryCache = System.Runtime.Caching.MemoryCache;

namespace Syhler.InformationGathering.Infrastructure
{
    public class CacheService : ICacheService
    {

        private ObjectCache _cache = MemoryCache.Default;  


        public Task<bool> Save<T>(string key, T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            
            _cache.Add(key, value, DateTimeOffset.Now.AddMinutes(10));
            
            return Task.FromResult(true);
        }

        public Task<bool> Save<T>(string key, T value, int duration)
        {
            throw new NotImplementedException();
            return Task.FromResult(false);

        }

        public Task<T> Get<T>(string key)
        {
            throw new NotImplementedException();
            return null;
        }

        public Task<bool> TryAndGet<T>(string key, out T value)
        {
            var result = (T)_cache.Get(key);
            value = result;

            return Task.FromResult(result != null);
        }
    }
}