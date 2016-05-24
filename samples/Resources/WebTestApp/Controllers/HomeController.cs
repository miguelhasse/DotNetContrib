using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public void ChangeCulture(string lang)
        {
            // Set culture to use next
            Global.SavePreferredCulture(lang);
            // Return to the calling URL (or go to the site's home page)
            HttpContext.Response.Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
        }
    }
}
