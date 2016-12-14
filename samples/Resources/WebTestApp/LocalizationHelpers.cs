using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebTestApp
{
    public static class LocalizationHelpers
    {
        public static string Resource(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            string virtualPath = GetVirtualPath(htmlhelper);
            return GetResourceString(htmlhelper.ViewContext.HttpContext, expression, virtualPath, args);
        }

        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            return GetResourceString(controller.HttpContext, expression, "~/", args);
        }

        public static void CopyFrom(this Hasseware.Resources.ResourceProviderManager resourceManager, ResourceManager manager, CultureInfo ci)
        {
            var writer = ((Hasseware.Resources.ResourceProviderSet)resourceManager.GetResourceSet(ci, true, false)).Writer;

            foreach (System.Collections.DictionaryEntry entry in manager.GetResourceSet(ci, true, true))
            {
                if (entry.Value is string)
                    writer.AddResource((string)entry.Key, (string)entry.Value);
            }
            writer.Generate();
        }

        public static string GetResourceJson(string classKey)
        {
            //var config = httpContext.GetSection("system.web/globalization") as GlobalizationSection;

            var ci = CultureInfo.CurrentUICulture;
            var enumerator = Hasseware.Web.Compilation.ResourceProviderFactory.GetGlobalResourceEnumerator(classKey, ci);

            if (enumerator != null)
            {
                var resources = new Dictionary<string,string>();

                while (enumerator.MoveNext())
                    resources.Add(enumerator.Key.ToString(), enumerator.Value.ToString());

                return (new JavaScriptSerializer()).Serialize(new { classkey = classKey, culture = ci.Name, resources });
            }
            return null;
        }

        private static string GetResourceString(HttpContextBase httpContext, string expression, string virtualPath, object[] args)
        {
            ResourceExpressionFields fields = ResourceExpressionBuilder.ParseExpression(expression);

            object formatObj = !String.IsNullOrEmpty(fields.ClassKey) ?
                httpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture) :
                httpContext.GetLocalResourceObject(virtualPath, fields.ResourceKey, CultureInfo.CurrentUICulture);

            return (formatObj is string) ? String.Format((string)formatObj, args) : fields.ResourceKey;
        }

        private static string GetVirtualPath(HtmlHelper htmlhelper)
        {
            var view = htmlhelper.ViewContext.View as BuildManagerCompiledView;
            return (view != null) ? view.ViewPath : null;
        }
    }
}
