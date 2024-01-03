using System.Collections.Generic;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class CountValidator<TModel, TCollection, TItem>
        : AbstractCountValidator<TModel, Optional<TCollection>>
        where TCollection : ICollection<TItem>
    {
        public CountValidator(int min, int max)
            : base(min, max)
        { }

        public override bool IsValid(ValidationContext<TModel> context, Optional<TCollection> optional)
        {
            if (!optional.HasValue(out TCollection? value)) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class MaximumCountValidator<TModel, TCollection, TItem>
        : CountValidator<TModel, TCollection, TItem>
        where TCollection : ICollection<TItem>
    {
        public MaximumCountValidator(int max)
         : base(min: 0, max: max)
        { }
    }

    public class MinimumCountValidator<TModel, TCollection, TItem>
        : CountValidator<TModel, TCollection, TItem>
        where TCollection : ICollection<TItem>
    {
        public MinimumCountValidator(int min)
        : base(min: min, max: -1)
        { }
    }
}