using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Menu/Index.cshtml");
        }
    }
}