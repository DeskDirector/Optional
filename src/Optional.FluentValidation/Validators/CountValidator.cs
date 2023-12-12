using System;
using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public abstract class CountValidatorBase<TModel, TProperty> : PropertyValidator<TModel, TProperty?>, ILengthValidator
    {
        public int Min { get; }

        public int Max { get; }

        public override string Name => "CollectionCountValidator";

        protected CountValidatorBase(int min, int max)
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

        protected bool IsCollectionValid(ValidationContext<TModel> context, int count)
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

    public class CountValidator<TModel, TCollection, TItem>(int min, int max)
        : CountValidatorBase<TModel, Optional<TCollection>>(min, max)
        where TCollection : ICollection<TItem>
    {
        public override bool IsValid(ValidationContext<TModel> context, Optional<TCollection> optional)
        {
            if (!optional.HasValue(out TCollection? value)) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class CountValidator<TModel, TItem>(int min, int max)
        : CountValidatorBase<TModel, OptionalCollection<TItem>>(min, max)
    {
        public override bool IsValid(ValidationContext<TModel> context, OptionalCollection<TItem> property)
        {
            if (!property.HasValue(out ICollection<TItem>? value)) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class CollectionCountValidator<TModel, TProperty>(int min, int max)
        : CountValidatorBase<TModel, TProperty>(min, max)
        where TProperty : ICollection
    {
        public override bool IsValid(ValidationContext<TModel> context, TProperty? value)
        {
            if (value is null) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class GenericCollectionCountValidator<TModel, TItem>(int min, int max)
        : CountValidatorBase<TModel, ICollection<TItem>?>(min, max)
    {
        public override bool IsValid(ValidationContext<TModel> context, ICollection<TItem>? value)
        {
            if (value is null) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class MaximumCountValidator<TModel, TCollection, TItem>(int max)
        : CountValidator<TModel, TCollection, TItem>(min: 0, max: max)
        where TCollection : ICollection<TItem>;

    public class MinimumCountValidator<TModel, TCollection, TItem>(int min)
        : CountValidator<TModel, TCollection, TItem>(min: min, max: -1)
        where TCollection : ICollection<TItem>;
}