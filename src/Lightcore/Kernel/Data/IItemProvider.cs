using System.Threading.Tasks;

namespace Lightcore.Kernel.Data
{
    public interface IItemProvider
    {
        Task<Item> GetItemAsync(string pathOrId, Language language);
    }
}