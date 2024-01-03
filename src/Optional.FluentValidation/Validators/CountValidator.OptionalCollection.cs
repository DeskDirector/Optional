using System.Collections.Generic;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class CountValidator<TModel, TItem>
        : AbstractCountValidator<TModel, OptionalCollection<TItem>>
    {
        public CountValidator(int min, int max)
            : base(min, max)
        { }

        public override bool IsValid(ValidationContext<TModel> context, OptionalCollection<TItem> property)
        {
            if (!property.HasValue(out ICollection<TItem>? value)) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class MaximumCountValidator<TModel, TItem>
        : CountValidator<TModel, TItem>
    {
        public MaximumCountValidator(int max)
            : base(min: 0, max: max)
        { }
    }

    public class MinimumCountValidator<TModel, TItem>
        : CountValidator<TModel, TItem>
    {
        public MinimumCountValidator(int min)
            : base(min: min, max: -1)
        { }
    }
}