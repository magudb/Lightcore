using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightcore.Kernel.Data.Providers
{
    public interface IItemProvider
    {
        Task<Item> GetItemAsync(GetItemCommand command);

        Task<IEnumerable<Item>> GetVersionsAsync(GetVersionsCommand command);
    }
}