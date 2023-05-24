using System;
using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public class OptionalNotEmptyValidator<TModel, TProperty, TItem> : PropertyValidator<TModel, TProperty>
        where TProperty : IOptional<TItem>
    {
        public override string Name => "OptionalNotEmptyValidator";

        public override bool IsValid(ValidationContext<TModel> context, TProperty? optional)
        {
            if (optional == null) {
                return true;
            }

            if (optional.IsUndefined()) {
                return true;
            }

            if (!optional.HasValue(out TItem? value)) {
                return false;
            }

            switch (value) {
                case null:
                case string s when String.IsNullOrWhiteSpace(s):
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return false;
            }

            return !EqualityComparer<TItem>.Default.Equals(value, default);
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} cannot be empty.";
    }
}