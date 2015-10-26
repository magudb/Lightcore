using System;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public class ItemDefinition : IItemDefinition
    {
        public ItemDefinition(Guid id, Guid templateId, string name, string path, Language language, bool hasVersion, FieldCollection fields)
        {
            Id = id;
            TemplateId = templateId;
            Name = name;
            Key = name.ToLowerInvariant();
            Path = path;
            Language = language;
            HasVersion = hasVersion;
            Fields = fields;
        }

        public string Path { get; }
        public string Name { get; }
        public string Key { get; }
        public Guid Id { get; }
        public Language Language { get; }
        public FieldCollection Fields { get; }
        public bool HasVersion { get; }
        public Guid TemplateId { get; }
        public string this[string fieldName] => Fields[fieldName]?.Value;
    }
}