using System;
using Lightcore.Kernel.Data.Fields;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public interface IItem
    {
        string Path { get; }
        string Name { get; }
        string Key { get; }
        Guid Id { get; }
        Language Language { get; }
        FieldCollection Fields { get; }
        bool HasVersion { get; }
        Guid TemplateId { get; }
        string this[string fieldName] { get; }
    }
}