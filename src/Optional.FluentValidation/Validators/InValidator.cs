using System;
using System.Collections.Generic;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class InValidator<T> : PropertyValidator
    {
        public HashSet<T> ValidSet { get; }

        public InValidator(HashSet<T> validSet)
            : base("{PropertyName} has invalid value '{PropertyValue}', Valid value set is: [{ValidSet}]")
        {
            ValidSet = validSet ?? throw new ArgumentNullException(nameof(validSet));
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            switch (context.PropertyValue) {
                case Optional<T> optional:
                    return IsValid(optional, context);

                case T value:
                    return IsValid(value, context);

                default:
                    return true;
            }
        }

        private bool IsValid(Optional<T> optional, PropertyValidatorContext context)
        {
            return !optional.HasValue(out T value) || IsValid(value, context);
        }

        private bool IsValid(T value, PropertyValidatorContext context)
        {
            if (value == null) {
                return true;
            }

            if (ValidSet.Contains(value)) {
                return true;
            }

            context.MessageFormatter
                .AppendArgument("ValidSet", String.Join(", ", ValidSet));

            return false;
        }
    }
}