using System;
using System.Linq;
using Lightcore.Kernel.Data.Globalization;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class ResolveLanguageProcessor : Processor<RequestArgs>
    {
        public override void Process(RequestArgs args)
        {
            var context = args.HttpContext.LightcoreContext();
            var languageSegment = args.HttpContext.Request.Path.Value.ToLowerInvariant().Split('/').Skip(1).FirstOrDefault();

            // Get current language from path
            if (!string.IsNullOrWhiteSpace(languageSegment)
                && (languageSegment.Equals("en", StringComparison.OrdinalIgnoreCase)
                    || languageSegment.Contains("-")))
            {
                // TODO: Make a better check to see if it is a valid "culture" segment

                context.Language = new Language(languageSegment);

                args.HttpContext.Request.Path = new PathString(args.HttpContext.Request.Path.Value.Replace("/" + languageSegment, ""));
            }
            else
            {
                // Or use the default language
                context.Language = Language.Default;
            }
        }
    }
}