using System.Collections.Generic;
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
        
        public RenderRenderingArgs GetArgs(HttpContext httpContext, RouteData routeData, Rendering rendering)
        {
            Requires.IsNotNull(rendering, nameof(rendering));
            
            return new RenderRenderingArgs(httpContext, routeData, rendering);
        }
    }
}