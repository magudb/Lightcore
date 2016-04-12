using System;
using System.Collections.Generic;

namespace Lightcore.Server.Models
{
    public class ItemModel
    {
        public ItemPropertyModel Properties { get; set; }
        public PresentationModel Presentation { get; set; }
        public IEnumerable<FieldModel> Fields { get; set; }
        public IEnumerable<ItemModel> Children { get; set; }
    }

    public class ItemModelV2
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string Language { get; set; }
        public string Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid TemplateId { get; set; }
        public bool HasVersion { get; set; }
        public PresentationModel Presentation { get; set; }
        public IEnumerable<FieldModel> Fields { get; set; }
        public IEnumerable<Guid> ChildIds { get; set; }
    }
}