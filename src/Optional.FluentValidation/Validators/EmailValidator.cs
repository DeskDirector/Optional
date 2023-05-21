using System;
using FluentValidation;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class EmailValidator<TModel> : PropertyValidator<TModel, IOptional<string>>
    {
        public override string Name => "OptionalEmailValidator";

        public override bool IsValid(ValidationContext<TModel> context, IOptional<string>? optional)
        {
            if (optional == null) {
                return true;
            }

            if (!optional.HasValue(out string? value)) {
                return true;
            }

            return IsEmail(value.AsSpan());
        }

        private static bool IsEmail(ReadOnlySpan<char> value)
        {
            ReadOnlySpan<char> input = value.IsEmpty ? ReadOnlySpan<char>.Empty : value.Trim();
            if (input.IsEmpty) {
                return false;
            }

            int indexOfAt = input.IndexOf('@');
            if (indexOfAt <= 0 || indexOfAt >= input.Length - 1) {
                return false;
            }

            int indexOfOtherAt = input[(indexOfAt + 1)..].IndexOf('@');
            if (indexOfOtherAt > 0) {
                return false;
            }

            int indexOfDoubleDot = input.IndexOf("..");
            return indexOfDoubleDot < 0;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) =>
            "{PropertyName} has to be email address";
    }
}