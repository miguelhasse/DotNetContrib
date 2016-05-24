using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Generic;
using System.Web.Http.Metadata;

namespace System.Web.Http.Validation
{
    public class EntLibModelValidator : ModelValidator
    {
        public EntLibModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, Validator validator)
            : base(validatorProviders)
        {
            ModelValidator = validator;
        }

        public Validator ModelValidator { get; private set; }

        public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object container)
        {
            return ConvertResults(ModelValidator.Validate(container ?? metadata.Model));
        }

        private IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> validationResults)
        {
            if (validationResults != null)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    if (validationResult.NestedValidationResults != null)
                    {
                        foreach (ModelValidationResult result in ConvertResults(validationResult.NestedValidationResults))
                            yield return result;
                    }
                    yield return new ModelValidationResult
                    {
                        MemberName = validationResult.Key,
                        Message = validationResult.Message
                    };
                }
            }
            yield break;
        }
    }
}
