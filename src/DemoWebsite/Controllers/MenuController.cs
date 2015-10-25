using System.Collections.Generic;
using System.Threading.Tasks;
using Lightcore.Kernel;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Providers;
using Microsoft.AspNet.Mvc;
using WebApp.Models;

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
            var root = await _itemProvider.GetItemAsync(new GetItemCommand(datasource, context.Language).OnlyChildFields("title"));
            var languages = await _itemProvider.GetVersionsAsync(new GetVersionsCommand(datasource));
            var navigation = new List<IItem>(new[] {root});

            navigation.AddRange(root.Children);

            var model = new MenuModel
            {
                MainNavigation = navigation,
                LanguageNavigation = languages
            };

            return View("/Views/Menu/Index.cshtml", model);
        }
    }
}