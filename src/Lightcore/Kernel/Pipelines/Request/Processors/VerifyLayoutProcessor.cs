using Lightcore.Kernel.Pipelines.Request.Processors.Exceptions;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class VerifyLayoutProcessor : Processor<RequestArgs>
    {
        public override void Process(RequestArgs args)
        {
            var context = args.HttpContext.LightcoreContext();

            if (string.IsNullOrEmpty(context.Item.PresentationDetails?.Layout?.Path))
            {
                throw new ItemLayoutNotDefinedException(context.Item);
            }
        }
    }
}