namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class CreateContextProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;

            requestArgs.HttpContext.Items["LCC"] = new Context();
        }
    }
}