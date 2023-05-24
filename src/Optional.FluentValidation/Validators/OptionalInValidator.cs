using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class OptionalInValidator<TModel, TOptional, T> : PropertyValidator<TModel, TOptional>
        where TOptional : IOptional<T>
    {
        internal ReadOnlyHashSet<T> ValidSet { get; }

        public override string Name => "OptionalInValidator";

        public OptionalInValidator(HashSet<T> validSet)
        {
            ArgumentNullException.ThrowIfNull(validSet);

            ValidSet = validSet ?? throw new ArgumentNullException(nameof(validSet));
        }

        protected override string GetDefaultMessageTemplate(string errorCode) =>
            "{PropertyName} has invalid value '{PropertyValue}', Valid value set is: [{ValidSet}]";

        public override bool IsValid(ValidationContext<TModel> context, TOptional? optional)
        {
            if (optional == null) {
                return true;
            }

            if (!optional.HasValue(out T? value)) {
                return true;
            }

            if (ValidSet.Contains(value)) {
                return true;
            }

            context
                .MessageFormatter
                .AppendArgument("ValidSet", String.Join(", ", ValidSet));

            return false;
        }
    }
}