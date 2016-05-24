using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Globalization;

namespace WebTestApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }

        protected void Application_AcquireRequestState()
        {
            var handler = Context.Handler as MvcHandler;

            if (handler != null)
            {
                string cultureName = handler.RequestContext.RouteData.Values["culture"] as string;
                var languageCookie = HttpContext.Current.Request.Cookies["culture"];

                if (languageCookie != null)
                    cultureName = languageCookie.Values["language"];

                if (cultureName != null)
                {
                    var cultureInfo = CultureInfo.CreateSpecificCulture(cultureName);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
                }
            }
        }

        public static void SavePreferredCulture(string language, int expireDays = 1)
        {
            var cookie = new HttpCookie("culture")
            {
                Expires = System.DateTime.Now.AddDays(expireDays),
                Shareable = true
            };
            cookie.Values["language"] = language;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}