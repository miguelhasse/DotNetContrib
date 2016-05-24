using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebTestApp.Controllers
{
    public class ResourcesController : ApiController
    {

        // GET: api/Resources/Model
        public IHttpActionResult Get(string id)
        {
            var ci = CultureInfo.CurrentUICulture;
            var enumerator = Hasseware.Web.Compilation.ResourceProviderFactory.GetGlobalResourceEnumerator(id, ci);

            if (enumerator != null)
            {
                var resources = new List<dynamic>();

                while (enumerator.MoveNext())
                    resources.Add(new { key = enumerator.Key, value = enumerator.Value });

                return Ok(new { classkey = id, culture = ci.Name, resources });
            }
            return NotFound();
        }
    }
}
