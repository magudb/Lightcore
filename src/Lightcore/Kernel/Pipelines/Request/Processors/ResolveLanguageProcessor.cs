using System.Linq;
using System.Text.RegularExpressions;
using Lightcore.Kernel.Data.Globalization;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipelines.Request.Processors
{
    public class ResolveLanguageProcessor : Processor<RequestArgs>
    {
        private static readonly Regex _validCulture = new Regex("^[a-zA-Z]{2,3}(?:-[a-zA-Z]{2,3}(?:-[a-zA-Z]{4})?)?$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public override void Process(RequestArgs args)
        {
            var context = args.HttpContext.LightcoreContext();
            var path = args.HttpContext.Request.Path.Value;
            var languageSegment = path.Split('/').Skip(1).FirstOrDefault();

            // Get current language from path
            if (_validCulture.IsMatch(languageSegment))
            {
                context.Language = Language.Parse(languageSegment.ToLowerInvariant());

                args.HttpContext.Request.Path = new PathString(path.Replace("/" + languageSegment, ""));
            }
            else
            {
                // Or use the default language
                context.Language = Language.Default;
            }
        }
    }
}