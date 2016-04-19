using System.Collections.Generic;

namespace Lightcore.Http.Models
{
    public class PresentationModel
    {
        public LayoutModel Layout { get; set; }
        public IEnumerable<RenderingModel> Renderings { get; set; }
    }
}