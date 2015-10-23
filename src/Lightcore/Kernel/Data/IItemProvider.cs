using System.Collections.Generic;
using System.Threading.Tasks;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public interface IItemProvider
    {
        Task<Item> GetItemAsync(GetItemCommand command);
    }

    public class GetItemCommand
    {
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
        public IEnumerable<string> ItemFields { get; set; }
        public IEnumerable<string> ChieldFields { get; set; }
    }
}