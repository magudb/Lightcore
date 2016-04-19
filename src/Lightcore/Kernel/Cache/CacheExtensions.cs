namespace Lightcore.Kernel.Cache
{
    public static class CacheExtensions
    {
        public static bool TryGet<T>(this ICache cache, object key, out T value)
        {
            object obj;

            if (cache.TryGet(key, out obj))
            {
                value = (T)obj;

                return true;
            }

            value = default(T);

            return false;
        }
    }
}