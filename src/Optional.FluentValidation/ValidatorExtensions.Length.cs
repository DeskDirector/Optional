using System;
using FluentValidation;
using Nness.Text.Json.Validation.Validators;

namespace Nness.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specified range. The
        /// range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> Length<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            int min,
            int max)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new LengthValidator(min, max));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is not equal to the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="exactLength"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> Length<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            int exactLength)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new ExactLengthValidator(exactLength));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is larger than the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumLength"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> MaximumLength<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            int maximumLength)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MaximumLengthValidator(maximumLength));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is less than the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumLength"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<string>> MinimumLength<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            int minimumLength)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MinimumLengthValidator(minimumLength));
        }
    }
}