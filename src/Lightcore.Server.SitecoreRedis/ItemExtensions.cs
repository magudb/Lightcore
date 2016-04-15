using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lightcore.Server.SitecoreRedis
{
    internal static class ItemExtensions
    {
        public static string LanguageVersionNames(this Item item)
        {
            var versions = new List<string>();

            foreach (var language in item.Languages)
            {
                var version = item.Database.GetItem(item.ID, language);

                if (version.Versions.Count > 0)
                {
                    versions.Add(version.Language.Name.ToLowerInvariant());
                }
            }

            return string.Join("|", versions);
        }

        public static string AsLowercaseGuid(this ID id)
        {
            return id.Guid.ToString().ToLowerInvariant();
        }

        public static string ToStorageKey(this ID id, Language language)
        {
            return ("version:" + id.Guid + ":" + language.Name).ToLowerInvariant();
        }
    }
}