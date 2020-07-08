using System;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class LengthValidator : PropertyValidator, ILengthValidator
    {
        public int Min { get; }

        public int Max { get; }

        public LengthValidator(int min, int max)
            : this(
                min: min,
                max: max,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(CountValidator<>)),
                errorSource: new LanguageStringSource(nameof(LengthValidator))
                )
        { }

        internal LengthValidator(int min, int max, IStringSource errorCodeSource, IStringSource errorSource)
            : base(errorSource)
        {
            Max = max;
            Min = min;

            if (max != -1 && max < min) {
                throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
            }

            Options.ErrorCodeSource = errorCodeSource;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is Optional<string> optional)) {
                return true;
            }

            if (!optional.HasValue(out string? value)) {
                return true;
            }

            int min = Min;
            int max = Max;

            int length = value.Length;

            if (length >= min && (max == -1 || length <= max)) {
                return true;
            }

            context.MessageFormatter
                .AppendArgument("MinLength", min)
                .AppendArgument("MaxLength", max)
                .AppendArgument("TotalLength", length);

            return false;
        }
    }

    public class ExactLengthValidator : LengthValidator
    {
        public ExactLengthValidator(int length)
            : base(
                min: length,
                max: length,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(ExactLengthValidator)),
                errorSource: new LanguageStringSource(nameof(ExactLengthValidator))
                )
        { }
    }

    public class MaximumLengthValidator : LengthValidator
    {
        public MaximumLengthValidator(int max)
            : base(
                min: 0,
                max: max,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(MaximumLengthValidator)),
                errorSource: new LanguageStringSource(nameof(MaximumLengthValidator))
                )
        { }
    }

    public class MinimumLengthValidator : LengthValidator
    {
        public MinimumLengthValidator(int min)
            : base(
                min: min,
                max: -1,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(MinimumLengthValidator)),
                errorSource: new LanguageStringSource(nameof(MinimumLengthValidator))
                )
        { }
    }
}