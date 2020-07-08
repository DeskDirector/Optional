using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class NotEmptyValidator<T> : PropertyValidator
    {
        public NotEmptyValidator()
            : base(new LanguageStringSource(nameof(NotEmptyValidator)))
        {
            Options.ErrorCodeSource = ValidatorHelper.ErrorCodeSource(typeof(NotEmptyValidator<>));
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is IOptional optional)) {
                return true;
            }

            if (optional.IsUndefined()) {
                return true;
            }

            if (optional.IsNull()) {
                return false;
            }

            switch (optional) {
                case Optional<T> normal:
                    return IsValid(normal);

                case OptionalCollection<T> collection:
                    return IsValid(collection);

                default:
                    throw new InvalidOperationException($"Type {optional.GetType().FullName} is not expected");
            }
        }

        private static bool IsValid(OptionalCollection<T> optional)
        {
            ICollection<T>? value = optional.Value;
            return value != null && value.Count > 0;
        }

        private static bool IsValid(Optional<T> optional)
        {
            T value = optional.Value;
            if (value == null) {
                return false;
            }

            if (typeof(T) == typeof(string)) {
                return IsValidString(value);
            }

            return !typeof(IEnumerable).IsAssignableFrom(typeof(T)) || IsValidCollection(value);
        }

        private static bool IsValidString(T value)
        {
            if (!(value is string str)) {
                throw new InvalidOperationException($"Value should be string, but it is not Type: {typeof(T).FullName}");
            }

            return !String.IsNullOrWhiteSpace(str);
        }

        private static bool IsValidCollection(T value)
        {
            if (value == null) {
                return false;
            }

            if (!(value is IEnumerable enumerable)) {
                throw new InvalidOperationException($"Value should be IEnumerable, but it is not. Type: {typeof(T).FullName}");
            }

            return enumerable.Cast<object>().Any();
        }
    }
}