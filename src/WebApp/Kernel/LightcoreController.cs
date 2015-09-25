using Microsoft.AspNet.Mvc;
using WebApp.Models;

namespace WebApp.Kernel
{
    public class LightcoreController : Controller
    {
        public ActionResult Render(string contentPath, string layoutPath)
        {
            var context = Context.LightcoreContext();
            var model = new HomeModel
            {
                Path = contentPath,
                Language = context.Language
            };

            return View(layoutPath, model);
        }
    }
}