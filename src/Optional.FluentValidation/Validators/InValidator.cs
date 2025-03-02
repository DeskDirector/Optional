using System.Collections.Frozen;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public sealed class InValidator<TModel, TProperty, T> : PropertyValidator<TModel, TProperty?>
    {
        public override string Name => "InValidator";

        public FrozenSet<T> ValidSet { get; }

        public InValidator(FrozenSet<T> validSet)
        {
            ArgumentNullException.ThrowIfNull(validSet);

            ValidSet = validSet ?? throw new ArgumentNullException(nameof(validSet));
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "{PropertyName} has invalid value '{PropertyValue}', Valid value set is: [{ValidSet}]";

        public override bool IsValid(ValidationContext<TModel> context, TProperty? value)
        {
            if (value is null) {
                return true;
            }

            bool isValid = value switch {
                Optional<T> optional => IsValid(optional),
                T t => IsValid(t),
                _ => true
            };

            if (isValid) {
                return true;
            }

            context
                .MessageFormatter
                .AppendArgument("ValidSet", String.Join(", ", ValidSet));

            return false;
        }

        private bool IsValid(T value)
        {
            return ValidSet.Contains(value);
        }

        private bool IsValid(Optional<T> value)
        {
            if (!value.HasValue(out T? content)) {
                return true;
            }

            return IsValid(content);
        }
    }
}