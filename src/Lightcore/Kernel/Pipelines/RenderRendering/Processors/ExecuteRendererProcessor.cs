using System.Threading.Tasks;
using Lightcore.Kernel.Mvc;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class ExecuteRendererProcessor : Processor<RenderRenderingArgs>
    {
        public override async Task ProcessAsync(RenderRenderingArgs args)
        {
            var runner = new ControllerRunner(args.Rendering.Controller, args.Rendering.Action, args.HttpContext, args.RouteData);

            await runner.Execute(args.Writer);
        }
    }
}