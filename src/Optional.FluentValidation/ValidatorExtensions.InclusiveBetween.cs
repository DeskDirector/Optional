using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Nness.Text.Json.Validation.Validators;

namespace Nness.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines an 'inclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> InclusiveBetween<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder,
            TProperty from,
            TProperty to)
        {
            return ruleBuilder.SetValidator(
                new InclusiveBetweenValidator<TProperty>(from, to, Comparer<TProperty>.Default)
            );
        }

        /// <summary>
        /// Defines an 'inclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> InclusiveBetween<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            string from,
            string to)
        {
            return ruleBuilder.SetValidator(
                new InclusiveBetweenValidator<string>(from, to, StringComparer.Ordinal)
            );
        }

        /// <summary>
        /// Defines an 'inclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <param name="comparer">String comparer</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> InclusiveBetween<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            string from,
            string to,
            [NotNull] StringComparer comparer)
        {
            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            return ruleBuilder.SetValidator(
                new InclusiveBetweenValidator<string>(from, to, comparer)
            );
        }

        /// <summary>
        /// Defines an 'inclusive between' validator on the current rule builder, but only for
        /// properties of types that implement IComparable. Validation will fail if the value of the
        /// property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <param name="comparer">Comparer for TProperty</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> InclusiveBetween<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder,
            TProperty from,
            TProperty to,
            [NotNull] IComparer<TProperty> comparer)
        {
            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            return ruleBuilder.SetValidator(
                new InclusiveBetweenValidator<TProperty>(from, to, comparer)
            );
        }
    }
}