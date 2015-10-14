namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class CreateContextProcessor : Processor<RequestArgs>
    {
        public override void Process(RequestArgs args)
        {
            args.HttpContext.Items["LCC"] = new Context();
        }
    }
}