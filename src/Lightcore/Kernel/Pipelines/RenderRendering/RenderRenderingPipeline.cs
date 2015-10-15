using System.Collections.Generic;
using System.IO;
using Lightcore.Kernel.Data;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Lightcore.Kernel.Pipelines.RenderRendering.Processors;

namespace Lightcore.Kernel.Pipelines.RenderRendering
{
    public class RenderRenderingPipeline : Pipeline<RenderRenderingArgs>
    {
        public override IEnumerable<Processor<RenderRenderingArgs>> GetProcessors()
        {
            yield return new ExecuteRendererProcessor();
        }
        
        public RenderRenderingArgs GetArgs(HttpContext httpContext, RouteData routeData, Rendering rendering, TextWriter writer)
        {
            Requires.IsNotNull(rendering, nameof(rendering));
            Requires.IsNotNull(writer, nameof(writer));

            return new RenderRenderingArgs(httpContext, routeData, rendering, writer);
        }
    }
}