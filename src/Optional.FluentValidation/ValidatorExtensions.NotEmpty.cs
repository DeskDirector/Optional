using FluentValidation;
using DeskDirector.Text.Json.Validation.Validators;

namespace DeskDirector.Text.Json.Validation
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
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new OptionalNotEmptyValidator<T, Optional<string>, string>());
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder. Validation will fail if the
        /// property empty collection or null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of T's property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> NotEmpty<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new OptionalNotEmptyValidator<T, Optional<TProperty>, TProperty>());
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder. Validation will fail if the
        /// property empty collection or null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of T's property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TProperty>> NotEmpty<T, TProperty>(
            this IRuleBuilder<T, OptionalCollection<TProperty>> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new OptionalNotEmptyValidator<T, OptionalCollection<TProperty>, ICollection<TProperty>>());
        }
    }
}