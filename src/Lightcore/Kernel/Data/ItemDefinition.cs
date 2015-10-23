﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Data.Presentation;

namespace Lightcore.Kernel.Data
{
    public class ItemDefinition
    {
        public ItemDefinition()
        {
            Fields = new FieldCollection();
            Children = Enumerable.Empty<ItemDefinition>();
        }

        public string Path { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public Guid Id { get; set; }
        public PresentationDetails Visualization { get; set; }
        public Language Language { get; set; }
        public IEnumerable<ItemDefinition> Children { get; set; }
        public FieldCollection Fields { get; set; }
        public bool HasVersion { get; set; }
        public Guid TemplateId { get; set; }
        public Guid ParentId { get; set; }
    }
}