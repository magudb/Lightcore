using System.Collections.Generic;

namespace Lightcore.Redis.Models
{
    public class ItemModel
    {
        public ItemPropertyModel Properties { get; set; }
        public PresentationModel Presentation { get; set; }
        public IEnumerable<FieldModel> Fields { get; set; }
        public IEnumerable<ItemModel> Children { get; set; }
    }
}