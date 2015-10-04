using System.Collections.Generic;

namespace Lightcore.Kernel.Data
{
    public class ItemVisualization
    {
        public Layout Layout { get; set; }

        public IEnumerable<Rendering> Renderings { get; set; }
    }
}