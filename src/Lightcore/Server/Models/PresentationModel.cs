using System.Collections.Generic;

namespace Lightcore.Server.Models
{
    public class PresentationModel
    {
        public LayoutModel Layout { get; set; }
        public IEnumerable<RenderingModel> Renderings { get; set; }
    }
}