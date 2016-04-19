using Lightcore.Kernel.Cache;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class AddOutputToCacheProcessor : Processor<RenderRenderingArgs>
    {
        private readonly ICache _cache;

        public AddOutputToCacheProcessor(ICache cache)
        {
            _cache = cache;
        }

        public override void Process(RenderRenderingArgs args)
        {
            if (!args.Rendering.Caching.Cacheable || string.IsNullOrEmpty(args.CacheKey))
            {
                return;
            }

            _cache.Set(args.CacheKey, args.CacheableOutput);
        }
    }
}