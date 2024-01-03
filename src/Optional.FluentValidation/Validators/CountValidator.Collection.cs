using System.Collections;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class CollectionCountValidator<TModel, TProperty>
        : AbstractCountValidator<TModel, TProperty?>
        where TProperty : ICollection
    {
        public CollectionCountValidator(int min, int max)
            : base(min, max)
        { }

        public override bool IsValid(ValidationContext<TModel> context, TProperty? value)
        {
            if (value is null) {
                return true;
            }

            return IsCollectionValid(context, value.Count);
        }
    }

    public class CollectionMaximumCountValidator<TModel, TProperty> : CollectionCountValidator<TModel, TProperty>
        where TProperty : ICollection
    {
        public CollectionMaximumCountValidator(int max)
            : base(min: 0, max: max)
        { }
    }

    public class CollectionMinimumCountValidator<TModel, TProperty> : CollectionCountValidator<TModel, TProperty>
        where TProperty : ICollection
    {
        public CollectionMinimumCountValidator(int min)
            : base(min: min, max: -1)
        { }
    }
}