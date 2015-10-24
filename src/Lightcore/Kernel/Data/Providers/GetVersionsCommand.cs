using System.Collections.Generic;

namespace Lightcore.Kernel.Data.Providers
{
    public class GetVersionsCommand
    {
        private readonly List<string> _itemFields = new List<string>();

        public GetVersionsCommand(string pathOrId)
        {
            Requires.IsNotNullOrEmpty(pathOrId, nameof(pathOrId));

            PathOrId = pathOrId;
        }

        public string PathOrId { get; }
        public IEnumerable<string> ItemFields => _itemFields;

        public GetVersionsCommand OnlyItemFields(params string[] fields)
        {
            _itemFields.AddRange(fields);

            return this;
        }
    }
}