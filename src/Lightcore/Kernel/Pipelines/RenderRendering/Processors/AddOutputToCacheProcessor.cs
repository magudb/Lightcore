using System;
using Microsoft.Extensions.Caching.Memory;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class AddOutputToCacheProcessor : Processor<RenderRenderingArgs>
    {
        private readonly IMemoryCache _cache;

        public AddOutputToCacheProcessor(IMemoryCache cache)
        {
            _cache = cache;
        }

        public override void Process(RenderRenderingArgs args)
        {
            if (!args.Rendering.Caching.Cacheable || string.IsNullOrEmpty(args.CacheKey))
            {
                return;
            }

            _cache.Set(args.CacheKey, args.CacheableOutput, new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.MaxValue));
        }
    }
}