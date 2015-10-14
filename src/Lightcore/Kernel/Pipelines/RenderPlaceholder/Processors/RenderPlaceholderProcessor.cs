using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lightcore.Kernel.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder.Processors
{
    public class RenderPlaceholderProcessor : Processor<RenderPlaceholderArgs>
    {
        public override async Task ProcessAsync(RenderPlaceholderArgs args)
        {
            var builder = new StringBuilder();
            var renderings = args.Item.Visualization.Renderings
                                 .Where(r => r.Placeholder.Equals(args.Name, StringComparison.OrdinalIgnoreCase));

            foreach (var rendering in renderings)
            {
                var runner = new ControllerRunner(rendering.Controller, rendering.Action, args.HttpContext, args.RouteData);
                var output = await runner.Execute();

                builder.Append(output);
            }

            args.Results = new HtmlString(builder.ToString());
        }
    }
}