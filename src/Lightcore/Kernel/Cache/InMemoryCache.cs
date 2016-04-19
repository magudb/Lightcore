using Microsoft.Extensions.Caching.Memory;

namespace Lightcore.Kernel.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }

        public bool TryGet(object key, out object value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Set(object key, object value, CacheEntryOptions options = null)
        {
            var memoryCacheOptions = new MemoryCacheEntryOptions();

            if (options != null)
            {
                if (options.Expires == null)
                {
                    return;
                }

                memoryCacheOptions.SetAbsoluteExpiration(options.Expires.Value);
            }

            _cache.Set(key, value, memoryCacheOptions);
        }
    }
}