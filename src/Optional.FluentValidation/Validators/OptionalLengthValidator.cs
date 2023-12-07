using System;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class OptionalLengthValidator<TModel> : PropertyValidator<TModel, Optional<string>>, ILengthValidator
    {
        public int Min { get; }

        public int Max { get; }

        public override string Name => "OptionalLengthValidator";

        public OptionalLengthValidator(int min, int max)
        {
            Max = max;
            Min = min;

            if (max != -1 && max < min) {
                throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
            }
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            if (Min <= 0) {
                return "{PropertyName} must not contain more than {MaxLength} chars.";
            }

            if (Max <= 0) {
                return "{PropertyName} must container at least {MinLength} chars.";
            }

            return "{PropertyName} must not contain more than {MaxLength} chars and at least {MinLength} chars";
        }

        public override bool IsValid(ValidationContext<TModel> context, Optional<string> optional)
        {
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

    public class ExactLengthValidator<TModel> : OptionalLengthValidator<TModel>
    {
        public override string Name => "OptionalExactLengthValidator";

        public ExactLengthValidator(int length)
            : base(min: length, max: length)
        { }
    }

    public class MaximumLengthValidator<TModel> : OptionalLengthValidator<TModel>
    {
        public override string Name => "OptionalMaximumLengthValidator";

        public MaximumLengthValidator(int max)
            : base(min: 0, max: max)
        { }
    }

    public class MinimumLengthValidator<TModel> : OptionalLengthValidator<TModel>
    {
        public override string Name => "OptionalMinimumLengthValidator";

        public MinimumLengthValidator(int min)
            : base(min: min, max: -1)
        { }
    }
}