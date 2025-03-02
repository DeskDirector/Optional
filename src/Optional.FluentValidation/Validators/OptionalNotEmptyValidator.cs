using System.Collections;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
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
                    return false;

                case IEnumerable e:
                    return HasElement(e);
            }

            return !EqualityComparer<TItem>.Default.Equals(value, default);
        }

        private static bool HasElement(IEnumerable e)
        {
            // ReSharper disable once NotDisposedResource
            IEnumerator enumerator = e.GetEnumerator();

            bool hasElement = enumerator.MoveNext();

            // IEnumerator<T> is disposable
            if (enumerator is IDisposable disposable) {
                disposable.Dispose();
            }

            return hasElement;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} cannot be empty.";
    }
}