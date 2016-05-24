using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    public class EntLibModelValidator : ModelValidator
    {
        public EntLibModelValidator(ModelMetadata metadata, ControllerContext controllerContext, Validator validator)
            : base(metadata, controllerContext)
        {
            ModelValidator = validator;
        }

        public Validator ModelValidator { get; private set; }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            return ConvertResults(ModelValidator.Validate(Metadata.Model));
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
