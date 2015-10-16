using System.Collections.Generic;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class GenerateCacheKeyProcessor : Processor<RenderRenderingArgs>
    {
        public override void Process(RenderRenderingArgs args)
        {
            var segments = new Dictionary<string, string>();

            if (!args.Rendering.Caching.Cacheable)
            {
                return;
            }

            segments.Add("controller", args.Rendering.Controller);
            segments.Add("action", args.Rendering.Action);
            segments.Add("ph", args.Rendering.Placeholder);
            segments.Add("lang", args.Item.Language.Name);

            if (args.Rendering.Caching.VaryByItem)
            {
                segments.Add("data", args.Rendering.Datasource);
            }

            if (args.Rendering.Caching.VaryByParm)
            {
                segments.Add("params", string.Join(",", args.Rendering.Parameters));
            }

            if (args.Rendering.Caching.VaryByQueryString)
            {
                segments.Add("query", string.Join(",", args.HttpContext.Request.Query));
            }

            args.CacheKey = "lc-html:" + string.Join("+", segments);
        }
    }
}