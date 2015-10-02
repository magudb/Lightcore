using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Http;
using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IItemProvider _itemProvider;

        public MenuController(IItemProvider itemProvider)
        {
            _itemProvider = itemProvider;
        }

        public async Task<ActionResult> Index()
        {
            var context = Context.LightcoreContext();
            var home = await _itemProvider.GetItemAsync("/sitecore/content/home", context.Language);

            return View("/Views/Menu/Index.cshtml", home);
        }
    }
}