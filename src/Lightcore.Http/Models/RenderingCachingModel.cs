namespace Lightcore.Http.Models
{
    public class RenderingCachingModel
    {
        public bool Cacheable { get; set; }
        public bool VaryByItem { get; set; }
        public bool VaryByParameter { get; set; }
        public bool VaryByQueryString { get; set; }
    }
}