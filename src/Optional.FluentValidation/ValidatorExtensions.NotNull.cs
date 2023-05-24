using FluentValidation;
using Nness.Text.Json.Validation.Validators;

namespace Nness.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines a 'not null' validator on the current rule builder. Validation will fail if the
        /// property is null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of T's property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> NotNull<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder)
            where TProperty : IOptional
        {
            return ruleBuilder.SetValidator(new OptionalNotNullValidator<T, TProperty>());
        }
    }
}