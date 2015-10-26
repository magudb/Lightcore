using Microsoft.AspNet.Mvc;

namespace Lightcore.Kernel.Mvc
{
    public class LightcoreController : Controller
    {
        public ActionResult Render(string path)
        {
            var context = HttpContext.LightcoreContext();

            return View(context.Item.PresentationDetails.Layout.Path, context.Item);
        }
    }
}