using System.Threading.Tasks;
using Lightcore.Kernel.Data.Providers;
using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IItemProvider _itemProvider;

        public UserController(IItemProvider itemProvider)
        {
            _itemProvider = itemProvider;
        }

        public async Task<IActionResult> GetUser()
        {
            var item = await _itemProvider.GetItemAsync(new GetItemCommand("/sitecore/content/home", "en"));

            return new JsonResult(new {Username = "Batman", Item = item.Path});
        }
    }
}