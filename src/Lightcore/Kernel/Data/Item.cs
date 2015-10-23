﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Data.Presentation;

namespace Lightcore.Kernel.Data
{
    public class Item
    {
        private readonly ItemDefinition _itemDefinition;
        private IEnumerable<ChildItem> _children;

        public Item(ItemDefinition itemDefinition)
        {
            _itemDefinition = itemDefinition;
        }

        public string Path => _itemDefinition.Path;
        public string Name => _itemDefinition.Name;
        public string Key => _itemDefinition.Key;
        public Guid Id => _itemDefinition.Id;
        public PresentationDetails Visualization => _itemDefinition.Visualization;
        public Language Language => _itemDefinition.Language;

        public IEnumerable<ChildItem> Children
        {
            get { return _children ?? (_children = _itemDefinition.Children.Select(c => new ChildItem(c))); }
        }

        public FieldCollection Fields => _itemDefinition.Fields;
        public bool HasVersion => _itemDefinition.HasVersion;
        public Guid TemplateId => _itemDefinition.TemplateId;
        public Guid ParentId => _itemDefinition.ParentId;
        public string this[string fieldName] => Fields[fieldName]?.Value;
    }
}