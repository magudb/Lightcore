using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lightcore.Kernel.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderPlaceholder.Processors
{
    public class RenderPlaceholderProcessor : Processor
    {
        public override async Task ProcessAsync(PipelineArgs args)
        {
            var placeholderArgs = (RenderPlaceholderArgs)args;
            var builder = new StringBuilder();
            var renderings =
                placeholderArgs.Item.Visualization.Renderings.Where(
                    r => r.Placeholder.Equals(placeholderArgs.Name, StringComparison.OrdinalIgnoreCase));

            foreach (var rendering in renderings)
            {
                var runner = new ControllerRunner(rendering.Controller, rendering.Action, placeholderArgs.HttpContext, placeholderArgs.RouteData);
                var output = await runner.Execute();

                builder.Append(output);
            }

            placeholderArgs.Results = new HtmlString(builder.ToString());
        }
    }
}