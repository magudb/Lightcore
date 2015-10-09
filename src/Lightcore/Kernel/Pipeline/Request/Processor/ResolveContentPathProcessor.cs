using Lightcore.Kernel.Http;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class ResolveContentPathProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;
            var context = args.Context.LightcoreContext();
            var contentPath = string.Format("{0}/{1}", requestArgs.Config.StartItem.TrimEnd('/'), args.Context.Request.Path.Value).ToLowerInvariant();

            const string mustStartWith = "/sitecore/content/";

            if (!contentPath.StartsWith(mustStartWith))
            {
                throw new InvalidContentPathException($"Path did not start with '{mustStartWith}' it was '{context.ContentPath}', please check your 'StartItem' setting.");
            }

            context.ContentPath = contentPath;
        }
    }
}