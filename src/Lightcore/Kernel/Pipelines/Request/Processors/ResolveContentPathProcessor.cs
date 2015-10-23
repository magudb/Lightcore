using Lightcore.Kernel.Pipelines.Request.Processors.Exceptions;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class ResolveContentPathProcessor : Processor<RequestArgs>
    {
        public override void Process(RequestArgs args)
        {
            var context = args.HttpContext.LightcoreContext();
            var contentPath = string.Format("{0}/{1}",
                args.Options.Sitecore.StartItem.TrimEnd('/'),
                args.HttpContext.Request.Path.Value.TrimStart('/'))
                                    .TrimEnd('/')
                                    .ToLowerInvariant();

            const string mustStartWith = "/sitecore/content/";

            if (!contentPath.StartsWith(mustStartWith))
            {
                throw new InvalidContentPathException(
                    $"Path did not start with '{mustStartWith}' it was '{context.ContentPath}', please check your 'StartItem' setting.");
            }

            context.ContentPath = contentPath;
        }
    }
}