using Lightcore.Kernel.Cache;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class RenderFromCacheProcessor : Processor<RenderRenderingArgs>
    {
        private readonly ICache _cache;

        public RenderFromCacheProcessor(ICache cache)
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

            if (!_cache.TryGet(args.CacheKey, out html))
            {
                return;
            }

            args.Output.Write(html);
            args.AbortPipeline();
        }
    }
}