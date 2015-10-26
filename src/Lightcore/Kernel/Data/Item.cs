using System;
using System.Collections.Generic;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Data.Presentation;

namespace Lightcore.Kernel.Data
{
    public class Item : IItemDefinition
    {
        private readonly IItemDefinition _definition;

        public Item(IItemDefinition definition, Guid parentId, IEnumerable<IItemDefinition> children, PresentationDetails details = null)
        {
            _definition = definition;

            ParentId = parentId;
            Children = children;
            PresentationDetails = details;
        }

        public Guid ParentId { get; }
        public IEnumerable<IItemDefinition> Children { get; set; }
        public PresentationDetails PresentationDetails { get; }
        public string Path => _definition.Path;
        public string Name => _definition.Name;
        public string Key => _definition.Key;
        public Guid Id => _definition.Id;
        public Language Language => _definition.Language;
        public FieldCollection Fields => _definition.Fields;
        public bool HasVersion => _definition.HasVersion;
        public Guid TemplateId => _definition.TemplateId;
        public string this[string fieldName] => _definition.Fields[fieldName]?.Value;
    }
}