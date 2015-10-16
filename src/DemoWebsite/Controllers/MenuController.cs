using System;
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

        public async Task<ActionResult> Index(string datasource)
        {
            var context = HttpContext.LightcoreContext();
            var root = await _itemProvider.GetItemAsync(datasource, context.Language);

            return View("/Views/Menu/Index.cshtml", root);
        }
    }
}