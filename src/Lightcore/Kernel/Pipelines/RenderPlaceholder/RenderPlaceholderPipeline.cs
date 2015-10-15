using System.Collections.Generic;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Pipelines.RenderPlaceholder.Processors;
using Lightcore.Kernel.Pipelines.RenderRendering;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder
{
    public class RenderPlaceholderPipeline : Pipeline<RenderPlaceholderArgs>
    {
        private readonly RenderRenderingPipeline _renderRenderingPipeline;

        public RenderPlaceholderPipeline(RenderRenderingPipeline renderRenderingPipeline)
        {
            _renderRenderingPipeline = renderRenderingPipeline;
        }

        public override IEnumerable<Processor<RenderPlaceholderArgs>> GetProcessors()
        {
            yield return new RenderPlaceholderProcessor(_renderRenderingPipeline);
        }

        public RenderPlaceholderArgs GetArgs(HttpContext httpContext, RouteData routeData, Item item, string name)
        {
            Requires.IsNotNull(item, nameof(item));
            Requires.IsNotNullOrEmpty(name, nameof(name));

            return new RenderPlaceholderArgs(httpContext, routeData, item, name);
        }
    }
}