using System.Threading.Tasks;
using Lightcore.Kernel;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Mvc;
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

        public async Task<ActionResult> Index(PresentationContext presentationContext)
        {
            var context = Context.LightcoreContext();
            var root = await _itemProvider.GetItemAsync(presentationContext.DataSource, context.Language);

            return View("/Views/Menu/Index.cshtml", root);
        }
    }
}