using System.Collections.Generic;
using System.IO;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Presentation;
using Lightcore.Kernel.Pipelines.RenderRendering.Processors;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.Caching.Memory;

namespace Lightcore.Kernel.Pipelines.RenderRendering
{
    public class RenderRenderingPipeline : Pipeline<RenderRenderingArgs>
    {
        private readonly IMemoryCache _cache;

        public RenderRenderingPipeline(IMemoryCache cache)
        {
            _cache = cache;
        }

        public override IEnumerable<Processor<RenderRenderingArgs>> GetProcessors()
        {
            yield return new GenerateCacheKeyProcessor();
            yield return new RenderFromCacheProcessor(_cache);
            yield return new ExecuteRendererProcessor();
            yield return new AddOutputToCacheProcessor(_cache);
        }

        public RenderRenderingArgs GetArgs(HttpContext httpContext, RouteData routeData, Item item, Rendering rendering, TextWriter output)
        {
            Requires.IsNotNull(rendering, nameof(rendering));
            Requires.IsNotNull(output, nameof(output));

            return new RenderRenderingArgs(httpContext, routeData, item, rendering, output);
        }
    }
}