using System;

namespace Lightcore.Kernel.Data.Fields
{
    public class Field
    {
        public Field(string key, string type, Guid id, string value)
        {
            Key = key;
            Type = type;
            Id = id;
            Value = value;
        }

        public string Key { get; set; }
        public string Type { get; set; }
        public Guid Id { get; set; }
        public string Value { get; }
    }
}