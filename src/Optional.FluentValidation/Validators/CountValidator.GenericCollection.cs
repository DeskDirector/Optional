using System.Collections.Generic;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class GenericCollectionCountValidator<TModel, TItem>
        : AbstractCountValidator<TModel, ICollection<TItem>?>
    {
        public GenericCollectionCountValidator(int min, int max)
            : base(min, max)
        { }

        public override bool IsValid(ValidationContext<TModel> context, ICollection<TItem>? value)
        {
            if (value is null) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class GenericCollectionMaximumCountValidator<TModel, TItem> : GenericCollectionCountValidator<TModel, TItem>
    {
        public GenericCollectionMaximumCountValidator(int max)
            : base(min: 0, max: max)
        { }
    }

    public class GenericCollectionMinimumCountValidator<TModel, TItem> : GenericCollectionCountValidator<TModel, TItem>
    {
        public GenericCollectionMinimumCountValidator(int min)
            : base(min: min, max: -1)
        { }
    }
}