using System;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation
{
    public class EmailValidator : PropertyValidator
    {
        public EmailValidator() : base("{PropertyName} is invalid email address")
        { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            switch (context.PropertyValue) {
                case Optional<string> optional:
                    return IsValid(optional);

                case string value:
                    return IsValid(value.AsSpan());

                default:
                    return true;
            }
        }

        private static bool IsValid(Optional<string> settable)
        {
            return !settable.HasValue(out string value) || IsValid(value.AsSpan());
        }

        private static bool IsValid(ReadOnlySpan<char> value)
        {
            ReadOnlySpan<char> input = value.IsEmpty ? ReadOnlySpan<char>.Empty : value.Trim();
            if (input.IsEmpty) {
                return false;
            }

            int indexOfAt = input.IndexOf('@');
            if (indexOfAt <= 0 || indexOfAt >= input.Length - 1) {
                return false;
            }

            int indexOfOtherAt = input.Slice(indexOfAt + 1).IndexOf('@');
            if (indexOfOtherAt > 0) {
                return false;
            }

            int indexOfDoubleDot = input.IndexOf("..");
            return indexOfDoubleDot < 0;
        }
    }
}