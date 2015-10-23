using System;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public class ChildItem
    {
        private readonly ItemDefinition _itemDefinition;

        public ChildItem(ItemDefinition itemDefinition)
        {
            _itemDefinition = itemDefinition;
        }

        public string Path => _itemDefinition.Path;
        public string Name => _itemDefinition.Name;
        public string Key => _itemDefinition.Key;
        public Guid Id => _itemDefinition.Id;
        public Language Language => _itemDefinition.Language;
        public FieldCollection Fields => _itemDefinition.Fields;
        public bool HasVersion => _itemDefinition.HasVersion;
        public Guid TemplateId => _itemDefinition.TemplateId;
        public string this[string fieldName] => Fields[fieldName]?.Value;
    }
}