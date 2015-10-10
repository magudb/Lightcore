namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class CreateContextProcessor : Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;

            requestArgs.HttpContext.Items["LCC"] = new Context();
        }
    }
}