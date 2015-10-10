using Lightcore.Kernel.Http;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class ResolveContentPathProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;
            var context = requestArgs.HttpContext.LightcoreContext();
            var contentPath = string.Format("{0}/{1}", requestArgs.Config.StartItem.TrimEnd('/'), requestArgs.HttpContext.Request.Path.Value.TrimStart('/')).TrimEnd('/').ToLowerInvariant();

            const string mustStartWith = "/sitecore/content/";

            if (!contentPath.StartsWith(mustStartWith))
            {
                throw new InvalidContentPathException($"Path did not start with '{mustStartWith}' it was '{context.RequestedContentPath}', please check your 'StartItem' setting.");
            }

            context.RequestedContentPath = contentPath;
        }
    }
}