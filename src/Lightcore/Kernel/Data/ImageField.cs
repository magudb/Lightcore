using System;

namespace Lightcore.Kernel.Data
{
    public class ImageField : Field
    {
        public ImageField(string key, string type, Guid id, string alt, string url) : base(key, type, id, url)
        {
            Alt = alt;
            Url = url;
        }

        public string Alt { get; }
        public string Url { get; }
    }
}