using System.Threading.Tasks;
using Lightcore.Kernel.Http;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class ResolveItemProcessor : Kernel.Pipeline.Processor
    {
        public override async Task ProcessAsync(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;
            var context = requestArgs.HttpContext.LightcoreContext();
            var item = await requestArgs.ItemProvider.GetItemAsync(context.RequestedContentPath, context.Language);

            if (item != null && item.HasVersion)
            {
                context.Item = item;
            }
            else
            {
                requestArgs.HttpContext.Response.StatusCode = 404;
                requestArgs.Abort();
            }
        }
    }
}