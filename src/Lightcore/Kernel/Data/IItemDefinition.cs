using System;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public interface IItemDefinition
    {
        Guid TemplateId { get; }
        Guid Id { get; }
        string Name { get; }
        string Key { get; }
        string Path { get; }
        Language Language { get; }
        bool HasVersion { get; }
        FieldCollection Fields { get; }
        string this[string fieldName] { get; }
    }
}