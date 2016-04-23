using Sitecore.Data;
using Sitecore.Globalization;

namespace Lightcore.Server.SitecoreRedis
{
    internal static class ItemExtensions
    {
        public static string ToStorageKey(this ID id, long snapshotVersion, Language language)
        {
            return snapshotVersion + ":version:" + id.Guid + ":" + language.Name.ToLowerInvariant();
        }
    }
}