using System.Collections.Generic;

namespace Lightcore.Kernel.Data
{
    public class Presentation
    {
        public Presentation(Layout layout, IEnumerable<Rendering> renderings)
        {
            Layout = layout;
            Renderings = renderings;
        }

        public Layout Layout { get; }

        public IEnumerable<Rendering> Renderings { get; }
    }
}