using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class CountValidator<TModel, TCollection, T> : PropertyValidator<TModel, IOptional<TCollection>>, ILengthValidator
        where TCollection : ICollection<T>
    {
        public int Min { get; }

        public int Max { get; }

        public override string Name => "OptionalListCountValidator";

        internal CountValidator(int min, int max)
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

        public override bool IsValid(ValidationContext<TModel> context, IOptional<TCollection>? optional)
        {
            if (optional == null) {
                return true;
            }

            if (!optional.HasValue(out TCollection? value)) {
                return true;
            }

            int count = value.Count;
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

    public class MaximumCountValidator<TModel, TCollection, T> : CountValidator<TModel, TCollection, T>
        where TCollection : ICollection<T>
    {
        public MaximumCountValidator(int max)
            : base(min: 0, max: max)
        { }
    }

    public class MinimumCountValidator<TModel, TCollection, T> : CountValidator<TModel, TCollection, T>
        where TCollection : ICollection<T>
    {
        public MinimumCountValidator(int min)
            : base(min: min, max: -1)
        { }
    }
}