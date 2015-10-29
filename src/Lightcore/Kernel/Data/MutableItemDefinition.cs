using System;
using System.Linq;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public class MutableItemDefinition : IItemDefinition
    {
        public MutableItemDefinition()
        {
            Fields = new FieldCollection(Enumerable.Empty<Field>());
            Language = Language.Default;
        }

        public Guid TemplateId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }
        public Language Language { get; set; }
        public bool HasVersion { get; set; }
        public FieldCollection Fields { get; set; }
        public string this[string fieldName] => Fields[fieldName]?.Value;
    }
}