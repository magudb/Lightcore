using Microsoft.AspNet.Mvc.Rendering;

namespace Lightcore.Kernel.Pipelines.RenderField.Processors
{
    public class RenderFieldProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var renderFieldArgs = (RenderFieldArgs)args;
            var field = renderFieldArgs.Field;

            renderFieldArgs.Results = new HtmlString(field.Value);
            renderFieldArgs.Raw = field.Value;
            renderFieldArgs.AbortPipeline();
        }
    }
}