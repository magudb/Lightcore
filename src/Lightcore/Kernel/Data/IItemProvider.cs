using System.Threading.Tasks;
using Lightcore.Kernel.Data.Globalization;

namespace Lightcore.Kernel.Data
{
    public interface IItemProvider
    {
        Task<Item> GetItemAsync(string pathOrId, Language language);
    }
}