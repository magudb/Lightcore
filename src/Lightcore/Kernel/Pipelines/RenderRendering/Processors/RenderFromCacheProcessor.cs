using Microsoft.Extensions.Caching.Memory;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class RenderFromCacheProcessor : Processor<RenderRenderingArgs>
    {
        private readonly IMemoryCache _cache;

        public RenderFromCacheProcessor(IMemoryCache cache)
        {
            _cache = cache;
        }

        public override void Process(RenderRenderingArgs args)
        {
            if (!args.Rendering.Caching.Cacheable || string.IsNullOrEmpty(args.CacheKey))
            {
                return;
            }

            string html;

            if (!_cache.TryGetValue(args.CacheKey, out html))
            {
                return;
            }

            args.Output.Write(html);
            args.AbortPipeline();
        }
    }
}