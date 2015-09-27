using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Http;
using Microsoft.AspNet.Http;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class ResolveLanguageProcessor : Kernel.Pipeline.Processor
    {
        private readonly IEnumerable<Language> _suportedLanguages;

        public ResolveLanguageProcessor()
        {
            _suportedLanguages = new List<Language>
            {
                new Language("en"),
                new Language("da-DK")
            };
        }

        public override void Process(PipelineArgs args)
        {
            var context = args.Context.LightcoreContext();
            var languageSegment = args.Context.Request.Path.Value.ToLowerInvariant().Split('/').Skip(1).FirstOrDefault();

            // Get current language from path
            if (!string.IsNullOrWhiteSpace(languageSegment) && _suportedLanguages.Any(l => l.Name.Equals(languageSegment, StringComparison.OrdinalIgnoreCase)))
            {
                context.Language = new Language(languageSegment);

                args.Context.Request.Path = new PathString(args.Context.Request.Path.Value.Replace("/" + languageSegment, ""));
            }
            else
            {
                // Or use the default language
                context.Language = Language.Default;
            }
        }
    }
}