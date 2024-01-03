using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class InclusiveBetweenValidator<TModel, T> : PropertyValidator<TModel, Optional<T>>
    {
        public T From { get; }

        public T To { get; }

        private readonly IComparer<T> _comparer;

        public override string Name => "OptionalInclusiveBetweenValidator";

        public InclusiveBetweenValidator(T from, T to, IComparer<T>? comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;

            int fromCompareTo = _comparer.Compare(from, to);

            if (fromCompareTo <= 0) {
                From = from;
                To = to;
            } else {
                From = to;
                To = from;
            }
        }

        public override bool IsValid(ValidationContext<TModel> context, Optional<T> optional)
        {
            if (!optional.HasValue(out T? value)) {
                return true;
            }

            if (IsBiggerAndEqualToFrom(value) && IsLessAndEqualToTo(value)) {
                return true;
            }

            context.MessageFormatter
                .AppendArgument("From", From)
                .AppendArgument("To", To);

            return false;
        }

        private bool IsBiggerAndEqualToFrom(T value)
        {
            int compareToFrom = _comparer.Compare(value, From);
            return compareToFrom > 0;
        }

        private bool IsLessAndEqualToTo(T value)
        {
            int compareToTo = _comparer.Compare(value, To);
            return compareToTo < 0;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} has to be inclusively between {From} and {To}.";
    }
}