using System;
using System.Collections.Generic;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class InclusiveBetweenValidator<T> : PropertyValidator
    {
        public T From { get; }

        public T To { get; }

        private readonly IComparer<T> _comparer;

        public InclusiveBetweenValidator(T from, T to, IComparer<T> comparer)
            : base(new LanguageStringSource(nameof(InclusiveBetweenValidator)))
        {
            From = from;
            To = to;
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

            Options.ErrorCodeSource = ValidatorHelper.ErrorCodeSource(typeof(InclusiveBetweenValidator<>));

            int fromCompareTo = _comparer.Compare(from, to);
            if (fromCompareTo >= 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(to),
                    $"To should be larger than from. {from} compare to {to} is<{fromCompareTo}>"
                );
            }
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is Optional<T> optional)) {
                return true;
            }

            if (!optional.HasValue(out T _)) {
                return true;
            }

            T value = optional.Value;
            if (value == null) {
                throw new InvalidOperationException("Optional return Null value while the state is HasValue");
            }

            int compareToFrom = _comparer.Compare(value, From);
            int compareToTo = _comparer.Compare(value, To);
            if (compareToFrom >= 0 &&
                compareToTo <= 0) {
                return true;
            }

            context.MessageFormatter
                .AppendArgument("From", From)
                .AppendArgument("To", To)
                .AppendArgument("Value", context.PropertyValue);

            return false;
        }
    }
}