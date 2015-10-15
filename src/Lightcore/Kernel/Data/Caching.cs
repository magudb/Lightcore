namespace Lightcore.Kernel.Data
{
    public class Caching
    {
        public bool Cacheable { get; set; }
        public bool VaryByItem { get; set; }
        public bool VaryByParm { get; set; }
        public bool VaryByQueryString { get; set; }
    }
}