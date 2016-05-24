using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace System.Web.Mvc
{
    public class EntLibModelValidatorProvider : AssociatedValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            string ruleset = GetRuleset(context, attributes);
            var validator = ValidationFactory.CreateValidator(metadata.ModelType, ruleset);

            if (validator != null)
            {
                yield return new EntLibModelValidator(metadata, context, validator);
            }
            yield break;
        }

        private static string GetRuleset(ControllerContext context, IEnumerable<Attribute> attributes)
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

        public static void Register()
        {
            if (!ModelValidatorProviders.Providers.Any(provider => provider is EntLibModelValidatorProvider))
                ModelValidatorProviders.Providers.Insert(0, new EntLibModelValidatorProvider());
        }
    }
}
