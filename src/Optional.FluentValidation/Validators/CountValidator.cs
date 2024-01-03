using System;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public abstract class AbstractCountValidator<TModel, TProperty> : PropertyValidator<TModel, TProperty?>, ILengthValidator
    {
        public int Min { get; }

        public int Max { get; }

        public override string Name => "CollectionCountValidator";

        protected AbstractCountValidator(int min, int max)
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
}