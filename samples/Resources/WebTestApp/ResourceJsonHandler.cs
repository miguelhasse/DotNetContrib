using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.TestWebApp
{
    public class ResourceJsonHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!String.Equals(context.Request.RequestType, "GET"))
                return;

            string path = context.Request.Path.TrimStart('/').Replace(".resjson", String.Empty);
            string resjson = LocalizationHelpers.GetResourceJson(path);

            context.Response.Clear();
            context.Response.ContentType = "text/json";
            context.Response.Write(resjson);
            context.Response.Flush();
        }
    }
}