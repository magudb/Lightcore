using System.Collections.Generic;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data.Providers
{
    public class GetItemCommand
    {
        private readonly List<string> _childFields = new List<string>();
        private readonly List<string> _itemFields = new List<string>();

        public GetItemCommand(string pathOrId, Language language)
        {
            Requires.IsNotNullOrEmpty(pathOrId, nameof(pathOrId));
            Requires.IsNotNull(language, nameof(language));

            PathOrId = pathOrId;
            Language = language;
        }

        public GetItemCommand(string pathOrId, string language) : this(pathOrId, Language.Parse(language))
        {
        }

        public string PathOrId { get; }
        public Language Language { get; }
        public IEnumerable<string> ItemFields => _itemFields;
        public IEnumerable<string> ChildFields => _childFields;

        public GetItemCommand OnlyChildFields(params string[] fields)
        {
            _childFields.AddRange(fields);

            return this;
        }

        public GetItemCommand OnlyItemFields(params string[] fields)
        {
            _itemFields.AddRange(fields);

            return this;
        }
    }
}