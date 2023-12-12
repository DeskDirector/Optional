using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public abstract class InValidatorBase<TModel, TProperty, T> : PropertyValidator<TModel, TProperty?>
    {
        internal ReadOnlyHashSet<T> ValidSet { get; }

        protected InValidatorBase(HashSet<T> validSet)
        {
            ArgumentNullException.ThrowIfNull(validSet);

            ValidSet = validSet ?? throw new ArgumentNullException(nameof(validSet));
        }

        protected override string GetDefaultMessageTemplate(string errorCode) =>
            "{PropertyName} has invalid value '{PropertyValue}', Valid value set is: [{ValidSet}]";

        protected bool IsInSet(ValidationContext<TModel> context, T? value)
        {
            if (value is null) {
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

    public class OptionalInValidator<TModel, TOptional, T>(HashSet<T> validSet) : InValidatorBase<TModel, TOptional, T>(validSet)
        where TOptional : IOptional<T>
    {
        public override string Name => "OptionalInValidator";

        public override bool IsValid(ValidationContext<TModel> context, TOptional? optional)
        {
            if (optional == null) {
                return true;
            }

            if (!optional.HasValue(out T? value)) {
                return true;
            }

            return IsInSet(context, value);
        }
    }

    public class InValidator<TModel, T>(HashSet<T> validSet) : InValidatorBase<TModel, T, T>(validSet)
    {
        public override string Name => "InValidator";

        public override bool IsValid(ValidationContext<TModel> context, T? value)
        {
            if (value is null) {
                return true;
            }

            return IsInSet(context, value);
        }
    }
}