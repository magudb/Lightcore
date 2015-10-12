using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class ResolveItemProcessor : Processor
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
                requestArgs.EndPipeline();
            }
        }
    }
}