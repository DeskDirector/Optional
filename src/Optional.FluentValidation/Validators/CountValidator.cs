using System;
using System.Collections;
using System.Collections.Generic;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class CountValidator<T> : PropertyValidator, ILengthValidator
    {
        private const string ErrorTemplate =
            "'{PropertyName}' collection must be between {MinCount} and {MaxCount} items. Collection has {TotalCount} items";

        public int Min { get; }

        public int Max { get; }

        public CountValidator(int min, int max)
            : this(
                min: min,
                max: max,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(CountValidator<>)),
                errorSource: new StaticStringSource(ErrorTemplate)
                )
        {
        }

        internal CountValidator(int min, int max, IStringSource errorCodeSource, IStringSource errorSource)
            : base(errorSource)
        {
            Max = max;
            Min = min;

            if (min < 0) {
                throw new ArgumentOutOfRangeException(nameof(min), "Min for list count should be bigger or equal to zero");
            }

            if (max != -1 && max < min) {
                throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
            }

            Options.ErrorCodeSource = errorCodeSource;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is IOptional optional)) {
                return true;
            }

            if (optional.IsNull() || optional.IsUndefined()) {
                return true;
            }

            if (optional is OptionalCollection<T> collection) {
                return IsValid(collection, context);
            }

            return IsValid(optional, context);
        }

        private bool IsValid(OptionalCollection<T> optional, PropertyValidatorContext context)
        {
            if (!optional.HasValue(out ICollection<T>? value)) {
                return true;
            }

            int count = value.Count;
            return IsValid(count, context);
        }

        private bool IsValid(IOptional optional, PropertyValidatorContext context)
        {
            if (!optional.HasValue(out object? value)) {
                return true;
            }

            if (!(value is ICollection collection)) {
                return true;
            }

            int count = collection.Count;
            return IsValid(count, context);
        }

        private bool IsValid(int count, PropertyValidatorContext context)
        {
            int min = Min;
            int max = Max;

            if (count >= min && (max == -1 || count <= max)) {
                return true;
            }

            context.MessageFormatter
                .AppendArgument("MinCount", min)
                .AppendArgument("MaxCount", max)
                .AppendArgument("TotalCount", count);

            return false;
        }
    }

    public class MaximumCountValidator<T> : CountValidator<T>
    {
        private const string ErrorTemplate =
            "The count of '{PropertyName}' must be {MaxCount} items or fewer. Collection has {TotalCount} items";

        public MaximumCountValidator(int max)
            : base(
                min: 0,
                max: max,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(MaximumCountValidator<>)),
                errorSource: new StaticStringSource(ErrorTemplate)
            )
        { }
    }

    public class MinimumCountValidator<T> : CountValidator<T>
    {
        private const string ErrorTemplate =
            "The count of '{PropertyName}' must be at least {MaxCount} items. Collection has {TotalCount} items";

        public MinimumCountValidator(int min)
            : base(
                min: min,
                max: -1,
                errorCodeSource: ValidatorHelper.ErrorCodeSource(typeof(MinimumCountValidator<>)),
                errorSource: new StaticStringSource(ErrorTemplate)
            )
        { }
    }
}