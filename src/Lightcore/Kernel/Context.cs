using Lightcore.Kernel.Data;

namespace Lightcore.Kernel
{
    public class Context
    {
        public Language Language { get; set; }
        public Item Item { get; set; }
        public string RequestedContentPath { get; set; }
    }
}