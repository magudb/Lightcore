using Sitecore.Data;
using Sitecore.Globalization;

namespace Lightcore.Server.SitecoreRedis
{
    internal static class KeyGenerator
    {
        public static string ToRedisKey(this ID id, Language language)
        {
            return ("item:" + id.ToShortID() + ":" + language.Name).ToLowerInvariant();
        }
    }
}