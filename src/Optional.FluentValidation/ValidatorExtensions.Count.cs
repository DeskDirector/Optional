using System;
using System.Collections;
using System.Collections.Generic;
using DeskDirector.Text.Json.Validation.Validators;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties Validation will fail if the count of the collection is
        /// outside the specified range, The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TCollection">Type of object that is collection of TProperty</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TCollection>> Count<T, TCollection>(
            this IRuleBuilder<T, Optional<TCollection>> ruleBuilder,
            int min,
            int max)
            where TCollection : IEnumerable
        {
            return ruleBuilder.SetValidator(new CountValidator<T, Optional<TCollection>>(min: min, max: max));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties Validation will fail if the count of the collection is
        /// outside the specified range, The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TItem">Type of object inside collection</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TItem>> Count<T, TItem>(
            this IRuleBuilder<T, OptionalCollection<TItem>> ruleBuilder,
            int min,
            int max)
        {
            return ruleBuilder.SetValidator(new CountValidator<T, OptionalCollection<TItem>>(min: min, max: max));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties Validation will fail if the count of the collection is
        /// outside the specified range, The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Property of validation target</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty?> Count<T, TProperty>(
            this IRuleBuilder<T, TProperty?> ruleBuilder,
            int min,
            int max)
            where TProperty : IEnumerable
        {
            return ruleBuilder.SetValidator(new CountValidator<T, TProperty?>(min: min, max: max));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// larger than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TItem">Type of object inside collection</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TItem>> MaximumCount<T, TItem>(
            this IRuleBuilder<T, OptionalCollection<TItem>> ruleBuilder,
            int maximumCount)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MaximumCountValidator<T, OptionalCollection<TItem>>(max: maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// larger than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TCollection">Type of object that is collection of TProperty</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TCollection>> MaximumCount<T, TCollection>(
            this IRuleBuilder<T, Optional<TCollection>> ruleBuilder,
            int maximumCount)
            where TCollection : IEnumerable
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MaximumCountValidator<T, Optional<TCollection>>(max: maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// larger than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Property of validation target</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty?> MaximumCount<T, TProperty>(
            this IRuleBuilder<T, TProperty?> ruleBuilder,
            int maximumCount)
            where TProperty : IEnumerable
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MaximumCountValidator<T, TProperty?>(max: maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// less than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TItem">Type of object inside collection</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TItem>> MinimumCount<T, TItem>(
            this IRuleBuilder<T, OptionalCollection<TItem>> ruleBuilder,
            int minimumCount)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MinimumCountValidator<T, OptionalCollection<TItem>>(min: minimumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// less than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TCollection">Type of object that is collection of TProperty</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TCollection>> MinimumCount<T, TCollection>(
            this IRuleBuilder<T, Optional<TCollection>> ruleBuilder,
            int minimumCount)
            where TCollection : IEnumerable
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MinimumCountValidator<T, Optional<TCollection>>(min: minimumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// less than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Property of validation target</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty?> MinimumCount<T, TProperty>(
            this IRuleBuilder<T, TProperty?> ruleBuilder,
            int minimumCount)
            where TProperty : IEnumerable
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new MinimumCountValidator<T, TProperty?>(min: minimumCount));
        }
    }
}