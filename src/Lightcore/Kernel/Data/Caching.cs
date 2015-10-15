namespace Lightcore.Kernel.Data
{
    public class Caching
    {
        public Caching(bool cacheable, bool varyByItem, bool varyByParm, bool varyByQueryString)
        {
            Cacheable = cacheable;
            VaryByItem = varyByItem;
            VaryByParm = varyByParm;
            VaryByQueryString = varyByQueryString;
        }

        public bool Cacheable { get; }
        public bool VaryByItem { get; }
        public bool VaryByParm { get; }
        public bool VaryByQueryString { get; }
    }
}