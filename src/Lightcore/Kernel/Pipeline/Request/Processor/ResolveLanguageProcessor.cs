using System.Linq;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Http;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class ResolveLanguageProcessor : Kernel.Pipeline.Processor
    {
        public override void Process(PipelineArgs args)
        {
            var requestArgs = (RequestArgs)args;
            var context = requestArgs.HttpContext.LightcoreContext();
            var languageSegment = requestArgs.HttpContext.Request.Path.Value.ToLowerInvariant().Split('/').Skip(1).FirstOrDefault();

            // Get current language from path
            if (!string.IsNullOrWhiteSpace(languageSegment) && languageSegment.Contains("-"))
            {
                context.Language = new Language(languageSegment);

                requestArgs.HttpContext.Request.Path = new PathString(requestArgs.HttpContext.Request.Path.Value.Replace("/" + languageSegment, ""));
            }
            else
            {
                // Or use the default language
                context.Language = Language.Default;
            }
        }
    }
}