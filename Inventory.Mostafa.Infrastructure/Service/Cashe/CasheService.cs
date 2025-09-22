using Inventory.Mostafa.Application.Abstraction.Cash;
using Inventory.Mostafa.Domain.Shared;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Service.Cashe
{
    public class CasheService: ICashService
    {
        private readonly IMemoryCache _cache;

        public CasheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out T? value))
            {
                return Task.FromResult<T?>(value);
            }
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan duration)
        {
            _cache.Set(key, value, duration);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
