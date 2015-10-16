using System.Collections.Generic;

namespace Lightcore.Server.Models
{
    public class RenderingModel
    {
        public string Placeholder { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Datasource { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public RenderingCachingModel Caching { get; set; }
    }
}