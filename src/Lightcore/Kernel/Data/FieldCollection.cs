using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lightcore.Kernel.Data
{
    public class FieldCollection : IEnumerable<Field>
    {
        private readonly IEnumerable<Field> _fields;

        public FieldCollection(IEnumerable<Field> fields)
        {
            _fields = fields;
        }

        public FieldCollection()
        {
            _fields = Enumerable.Empty<Field>();
        }

        public Field this[string fieldName]
        {
            get { return _fields.FirstOrDefault(f => f.Key.Equals(fieldName, StringComparison.OrdinalIgnoreCase)); }
        }

        public IEnumerator<Field> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}