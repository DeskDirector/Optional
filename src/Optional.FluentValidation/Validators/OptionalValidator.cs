using FluentValidation;
using FluentValidation.Results;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class OptionalValidator<T> : AbstractValidator<Optional<T>>
    {
        private readonly IValidator<T> _validator;

        public OptionalValidator(IValidator<T> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        protected override bool PreValidate(ValidationContext<Optional<T>> context, ValidationResult result)
        {
            Optional<T> instance = context.InstanceToValidate;
            if (instance.IsUndefined()) {
                return false;
            }

            if (!instance.HasValue(out T? value)) {
                return true;
            }

            ValidationContext<T> internalContext = new(value, context.PropertyChain, context.Selector);
            ValidationResult internalResult = _validator.Validate(internalContext);
            foreach (ValidationFailure failure in internalResult.Errors) {
                result.Errors.Add(failure);
            }

            return true;
        }
    }
}