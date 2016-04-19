using System;

namespace Lightcore.Http.Models
{
    public class FieldModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
    }
}