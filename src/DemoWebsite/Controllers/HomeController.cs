using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Home.Index");
        }
    }
}