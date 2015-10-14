using System.Threading.Tasks;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class ResolveItemProcessor : Processor<RequestArgs>
    {
        public override async Task ProcessAsync(RequestArgs args)
        {
            var context = args.HttpContext.LightcoreContext();
            var item = await args.ItemProvider.GetItemAsync(context.RequestedContentPath, context.Language);

            if (item != null && item.HasVersion)
            {
                context.Item = item;
            }
            else
            {
                args.HttpContext.Response.StatusCode = 404;
                args.EndPipeline();
            }
        }
    }
}