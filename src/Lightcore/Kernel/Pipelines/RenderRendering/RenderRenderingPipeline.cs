using System.Collections.Generic;
using System.IO;
using Lightcore.Kernel.Cache;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Presentation;
using Lightcore.Kernel.Pipelines.RenderRendering.Processors;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderRendering
{
    public class RenderRenderingPipeline : Pipeline<RenderRenderingArgs>
    {
        private readonly ICache _cache;

        public RenderRenderingPipeline(ICache cache)
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