using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class OptionalNotNullValidator<TModel, TProperty> : PropertyValidator<TModel, TProperty>
      where TProperty : IOptional
    {
        public override string Name => "OptionalNotNullValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} cannot be null";

        public override bool IsValid(ValidationContext<TModel> context, TProperty? optional)
        {
            if (optional == null) {
                return true;
            }

            return !optional.IsNull();
        }
    }
}