﻿using Lightcore.Kernel;
using Microsoft.AspNet.Mvc;

namespace WebApp.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index()
        {
            var context = HttpContext.LightcoreContext();

            return View("/Views/Article/Index.cshtml", context.Item);
        }
    }
}