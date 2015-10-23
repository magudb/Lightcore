using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel
{
    public class Context
    {
        public Language Language { get; set; }
        public Item Item { get; set; }
        public string ContentPath { get; set; }
    }
}