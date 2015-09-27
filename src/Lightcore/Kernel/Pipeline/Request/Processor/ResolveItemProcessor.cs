using System.Threading.Tasks;
using Lightcore.Kernel.Http;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class ResolveItemProcessor : Kernel.Pipeline.Processor
    {
        public override async Task ProcessAsync(PipelineArgs args)
        {
            var context = args.Context.LightcoreContext();

            var item = await args.ItemProvider.GetItem(args.Context.Request.Path.Value, context.Language);

            if (item != null)
            {
                context.Item = item;
            }
            else
            {
                args.Context.Response.StatusCode = 404;
                args.Abort();
            }
        }
    }
}