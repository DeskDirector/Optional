using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class NotEmptyValidator<TModel, TCollection, T> : PropertyValidator<TModel, IOptional<TCollection>>
        where TCollection : IEnumerable<T>
    {
        public override string Name => "OptionalNotEmptyValidator";

        public override bool IsValid(ValidationContext<TModel> context, IOptional<TCollection>? optional)
        {
            if (optional == null) {
                return true;
            }

            if (optional.IsUndefined()) {
                return true;
            }

            TCollection? value = optional.Value;
            if (value == null) {
                return false;
            }

            return value switch {
                string str => str.Length > 0,
                ICollection<T> collection => collection.Count > 0,
                _ => value.Any()
            };
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} cannot be empty.";
    }
}