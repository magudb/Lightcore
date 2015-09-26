using Lightcore.Kernel.Http;
using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class FooterController : Controller
    {
        public ActionResult Index()
        {
            var context = Context.LightcoreContext();

            return View("/Views/Footer/Index.cshtml", context.Item);
        }
    }
}