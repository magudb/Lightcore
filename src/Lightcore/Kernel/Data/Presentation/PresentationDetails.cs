using System.Collections.Generic;

namespace Lightcore.Kernel.Data.Presentation
{
    public class PresentationDetails
    {
        public PresentationDetails(Layout layout, IEnumerable<Rendering> renderings)
        {
            Layout = layout;
            Renderings = renderings;
        }

        public Layout Layout { get; }

        public IEnumerable<Rendering> Renderings { get; }
    }
}