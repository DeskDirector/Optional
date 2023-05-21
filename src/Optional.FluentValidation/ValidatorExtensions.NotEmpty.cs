using System;
using System.Collections.Generic;
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
        public static IRuleBuilderOptions<T, IOptional<string>> NotEmpty<T>(
            this IRuleBuilder<T, IOptional<string>> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new NotEmptyValidator<T, string, char>());
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder. Validation will fail if the
        /// property empty collection or null. It is used for required field. Either undefined or required.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TItem">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, IOptional<ICollection<TItem>>> NotEmpty<T, TItem>(
            this IRuleBuilder<T, IOptional<ICollection<TItem>>> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new NotEmptyValidator<T, ICollection<TItem>, TItem>());
        }
    }
}