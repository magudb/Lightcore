using System;
using System.Linq;
using System.Threading.Tasks;
using Lightcore.Kernel.Pipelines.RenderRendering;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder.Processors
{
    public class RenderPlaceholderProcessor : Processor<RenderPlaceholderArgs>
    {
        private readonly RenderRenderingPipeline _renderRenderingPipeline;

        public RenderPlaceholderProcessor(RenderRenderingPipeline renderRenderingPipeline)
        {
            _renderRenderingPipeline = renderRenderingPipeline;
        }

        public override async Task ProcessAsync(RenderPlaceholderArgs args)
        {
            var renderings = args.Item.PresentationDetails.Renderings
                                 .Where(r => r.Placeholder.Equals(args.Name, StringComparison.OrdinalIgnoreCase));

            foreach (var renderArgs in renderings.Select(r => new RenderRenderingArgs(args.HttpContext, args.RouteData, args.Item, r, args.Output)))
            {
                await _renderRenderingPipeline.RunAsync(renderArgs);
            }
        }
    }
}