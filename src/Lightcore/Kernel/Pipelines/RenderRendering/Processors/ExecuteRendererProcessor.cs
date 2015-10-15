using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Mvc;

namespace Lightcore.Kernel.Pipelines.RenderRendering.Processors
{
    public class ExecuteRendererProcessor : Processor<RenderRenderingArgs>
    {
        public override async Task ProcessAsync(RenderRenderingArgs args)
        {
            var runner = new ControllerRunner(args.Rendering.Controller, args.Rendering.Action, args.HttpContext, args.RouteData);

            using (var writer = new StringWriter())
            {
                await runner.Execute(writer);

                args.CacheableOutput = writer.ToString();
                args.Output.Write(args.CacheableOutput);
            }
        }
    }
}