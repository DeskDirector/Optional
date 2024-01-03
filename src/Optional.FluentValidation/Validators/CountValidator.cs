using System;
using System.Collections;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class CountValidator<TModel, TProperty> : PropertyValidator<TModel, TProperty>, ILengthValidator
    {
        public int Min { get; }

        public int Max { get; }

        public override string Name => "CollectionCountValidator";

        public CountValidator(int min, int max)
        {
            Max = max;
            Min = min;

            if (min < 0) {
                throw new ArgumentOutOfRangeException(nameof(min), "Min for list count should be bigger or equal to zero");
            }

            if (max != -1 && max <= min) {
                throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
            }
        }

        public override bool IsValid(ValidationContext<TModel> context, TProperty value)
        {
            int? count = value switch {
                IOptional optional => GetCount(optional),
                ICollection collection => collection.Count,
                _ => null
            };

            if (count == null) {
                return true;
            }

            return IsCollectionValid(context, count.Value);
        }

        private static int? GetCount(IOptional optional)
        {
            if (!optional.HasValue(out object? value)) {
                return null;
            }

            return value switch {
                ICollection collection => collection.Count,
                _ => null
            };
        }

        private bool IsCollectionValid(ValidationContext<TModel> context, int count)
        {
            if (count >= Min && (Max == -1 || count <= Max)) {
                return true;
            }

            if (Min > 0) {
                _ = context.MessageFormatter.AppendArgument("MinElements", Min);
            }

            if (Max > 0) {
                _ = context.MessageFormatter.AppendArgument("MaxElements", Max);
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            if (Min <= 0) {
                return "{PropertyName} must not contain more than {MaxElements} items.";
            }

            if (Max <= 0) {
                return "{PropertyName} must container at least {MinElements} items.";
            }

            return "{PropertyName} must not contain more than {MaxElements} items and at least {MinElements} items";
        }
    }

    public sealed class MaximumCountValidator<TModel, TProperty> : CountValidator<TModel, TProperty>
    {
        public MaximumCountValidator(int max)
            : base(min: 0, max: max)
        { }
    }

    public sealed class MinimumCountValidator<TModel, TProperty> : CountValidator<TModel, TProperty>
    {
        public MinimumCountValidator(int min)
            : base(min: min, max: -1)
        { }
    }
}