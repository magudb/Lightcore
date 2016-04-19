namespace Lightcore.Kernel.Cache
{
    public class NullCache : ICache
    {
        public void Remove(object key)
        {
        }

        public bool TryGet(object key, out object value)
        {
            value = null;

            return false;
        }

        public void Set(object key, object value, CacheEntryOptions options = null)
        {
        }
    }
}