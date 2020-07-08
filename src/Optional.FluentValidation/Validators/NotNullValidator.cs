using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class NotNullValidator : PropertyValidator
    {
        public NotNullValidator()
            : base(new LanguageStringSource(nameof(NotNullValidator)))
        {
            Options.ErrorCodeSource = ValidatorHelper.ErrorCodeSource(typeof(NotNullValidator));
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is IOptional optional)) {
                return true;
            }

            if (optional.IsUndefined()) {
                return true;
            }

            return !optional.IsNull();
        }
    }
}