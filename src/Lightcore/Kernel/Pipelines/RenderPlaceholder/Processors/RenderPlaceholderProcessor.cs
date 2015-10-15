using System;
using System.Linq;
using System.Text;
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
            var builder = new StringBuilder();
            var renderings = args.Item.Visualization.Renderings
                                 .Where(r => r.Placeholder.Equals(args.Name, StringComparison.OrdinalIgnoreCase));

            foreach (var renderRenderingArgs in renderings.Select(rendering => new RenderRenderingArgs(args.HttpContext, args.RouteData, rendering)))
            {
                await _renderRenderingPipeline.RunAsync(renderRenderingArgs);

                builder.Append(renderRenderingArgs.Results);
            }

            args.Results = builder.ToString();
        }
    }
}