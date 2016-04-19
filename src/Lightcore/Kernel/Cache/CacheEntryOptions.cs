using System;

namespace Lightcore.Kernel.Cache
{
    public class CacheEntryOptions
    {
        public DateTimeOffset? Expires { get; private set; }

        public CacheEntryOptions SetAbsoluteExpiration(DateTimeOffset expires)
        {
            Expires = expires;

            return this;
        }
    }
}