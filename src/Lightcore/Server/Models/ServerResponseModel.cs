using System.Collections.Generic;

namespace Lightcore.Server.Models
{
    public class ServerResponseModel
    {
        public ItemModel Item { get; set; }
        public PresentationModel Presentation { get; set; }
        public IEnumerable<FieldModel> Fields { get; set; }
        public IEnumerable<ServerResponseModel> Children { get; set; }
    }
}