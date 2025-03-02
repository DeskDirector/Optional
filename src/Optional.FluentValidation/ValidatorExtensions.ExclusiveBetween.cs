using DeskDirector.Text.Json.Validation.Validators;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines an 'exclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside the specified range. The range is exclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> ExclusiveBetween<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder,
            TProperty from,
            TProperty to)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(
                new ExclusiveBetweenValidator<T, TProperty>(from, to, Comparer<TProperty>.Default)
            );
        }

        /// <summary>
        /// Defines an 'exclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside the specified range. The range is exclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> ExclusiveBetween<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            string from,
            string to)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(
                new ExclusiveBetweenValidator<T, string>(from, to, StringComparer.Ordinal)
            );
        }

        /// <summary>
        /// Defines an 'exclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside the specified range. The range is exclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <param name="comparer">String comparer</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> ExclusiveBetween<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            string from,
            string to,
            StringComparer comparer)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentNullException.ThrowIfNull(comparer);

            return ruleBuilder.SetValidator(
                new ExclusiveBetweenValidator<T, string>(from, to, comparer)
            );
        }

        /// <summary>
        /// Defines an 'exclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside the specified range. The range is exclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <param name="comparer">Comparer for TProperty</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> ExclusiveBetween<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder,
            TProperty from,
            TProperty to,
            IComparer<TProperty> comparer)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentNullException.ThrowIfNull(comparer);

            return ruleBuilder.SetValidator(
                new ExclusiveBetweenValidator<T, TProperty>(from, to, comparer)
            );
        }
    }
}