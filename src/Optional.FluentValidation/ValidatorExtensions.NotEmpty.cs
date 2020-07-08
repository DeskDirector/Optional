using System.Collections;
using FluentValidation;
using Nness.Text.Json.Validation.Validators;

namespace Nness.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder. Validation will fail if the
        /// property is empty string or null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> NotEmpty<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NotEmptyValidator<string>());
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder. Validation will fail if the
        /// property empty collection or null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> NotEmpty<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder)
            where TProperty : IEnumerable
        {
            return ruleBuilder.SetValidator(new NotEmptyValidator<TProperty>());
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder. Validation will fail if the
        /// property empty collection or null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TProperty>> NotEmpty<T, TProperty>(
            this IRuleBuilder<T, OptionalCollection<TProperty>> ruleBuilder)
            where TProperty : IEnumerable
        {
            return ruleBuilder.SetValidator(new NotEmptyValidator<TProperty>());
        }
    }
}