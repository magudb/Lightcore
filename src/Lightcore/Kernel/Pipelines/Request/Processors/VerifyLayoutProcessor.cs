using Lightcore.Kernel.Pipelines.Request.Processors.Exceptions;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class VerifyLayoutProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;
            var context = requestArgs.HttpContext.LightcoreContext();

            if (string.IsNullOrEmpty(context.Item.Visualization?.Layout?.Path))
            {
                throw new ItemLayoutNotDefinedException(context.Item);
            }
        }
    }
}