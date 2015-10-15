namespace Lightcore.Server.Models
{
    public class RenderingCachingModel
    {
        public bool Cacheable { get; set; }
        public bool VaryByItem { get; set; }
        public bool VaryByParm { get; set; }
        public bool VaryByQueryString { get; set; }
    }
}