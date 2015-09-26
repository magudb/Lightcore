using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Http;
using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var context = Context.LightcoreContext();
            var itemProvider = new ItemProvider();
            var home = await itemProvider.GetItem("/sitecore/content/home", context.Language);
            
            return View("/Views/Menu/Index.cshtml", home);
        }
    }
}