namespace Lightcore.Kernel.Cache
{
    public interface ICache
    {
        void Remove(object key);

        bool TryGet(object key, out object value);

        void Set(object key, object value, CacheEntryOptions options = null);
    }
}