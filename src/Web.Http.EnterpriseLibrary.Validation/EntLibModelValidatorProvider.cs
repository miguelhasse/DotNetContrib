using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Metadata;
using System.Web.Routing;

namespace System.Web.Http.Validation.Providers
{
    public class EntLibModelValidatorProvider : AssociatedValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, IEnumerable<Attribute> attributes)
        {
            string ruleset = GetRuleset(HttpContext.Current.Request.RequestContext, attributes);
            var validator = ValidationFactory.CreateValidator(metadata.ModelType, ruleset);

            if (validator != null)
            {
                yield return new EntLibModelValidator(validatorProviders, validator);
            }
            yield break;
        }

        private static string GetRuleset(RequestContext context, IEnumerable<Attribute> attributes)
        {
            //TODO: verify support for ruleset selection
            string ruleset = context.RouteData.DataTokens["ruleset"] as string;

            if (ruleset == null && attributes != null)
            {
                ruleset = attributes.Where(attrib => attrib is UIHintAttribute).Cast<UIHintAttribute>()
                    .Select(attrib => attrib.PresentationLayer).FirstOrDefault();
            }
            return ruleset;
        }
    }
}
