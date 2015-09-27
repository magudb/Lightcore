using System.Threading.Tasks;

namespace Lightcore.Kernel.Data
{
    public interface IItemProvider
    {
        Task<Item> GetItem(string pathOrId, Language language);
    }
}