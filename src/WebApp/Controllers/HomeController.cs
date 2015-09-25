using Microsoft.AspNet.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string path)
        {
            var light = Context.LightcoreContext();
            var model = new HomeModel
            {
                Path = path,
                Language = light.Language
            };

            return View("/Views/Layout.cshtml", model);
        }
    }
}