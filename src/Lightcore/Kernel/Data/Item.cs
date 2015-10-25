using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Data.Presentation;

namespace Lightcore.Kernel.Data
{
    public class Item : IItem
    {
        private readonly ItemDefinition _itemDefinition;
        private IEnumerable<ChildItem> _children;

        public Item(ItemDefinition itemDefinition)
        {
            _itemDefinition = itemDefinition;
        }

        public PresentationDetails Visualization => _itemDefinition.Visualization;

        public IEnumerable<ChildItem> Children
        {
            get { return _children ?? (_children = _itemDefinition.Children.Select(c => new ChildItem(c))); }
        }

        public Guid ParentId => _itemDefinition.ParentId;

        public string Path => _itemDefinition.Path;
        public string Name => _itemDefinition.Name;
        public string Key => _itemDefinition.Key;
        public Guid Id => _itemDefinition.Id;
        public Language Language => _itemDefinition.Language;
        public FieldCollection Fields => _itemDefinition.Fields;
        public bool HasVersion => _itemDefinition.HasVersion;
        public Guid TemplateId => _itemDefinition.TemplateId;
        public string this[string fieldName] => _itemDefinition.Fields[fieldName]?.Value;
    }
}