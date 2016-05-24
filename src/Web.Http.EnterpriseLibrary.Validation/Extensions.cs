using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;

namespace System.Web.Http
{
    public static class Extensions
    {
        public static void RegisterEntLibModelValidator(this HttpConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            var providers = configuration.Services.GetModelValidatorProviders();
            if (!providers.Any(provider => provider is EntLibModelValidatorProvider))
            {
                var services = new List<ModelValidatorProvider>(providers);
                services.Insert(0, new EntLibModelValidatorProvider());

                configuration.Services.ReplaceRange(typeof(ModelValidatorProvider), services);
            }
        }
    }
}
