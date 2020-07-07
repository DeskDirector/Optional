using System;
using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using Nness.Text.Json.Validation.Validators;

namespace Nness.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {/// <summary>
     /// Defines a count validator on the current rule builder, but only for <see
     /// cref="ICollection{T}"/> properties Validation will fail if the count of the collection is
     /// outside of the specified range, The range is inclusive.
     /// </summary>
     /// <typeparam name="T">Type of object being validated</typeparam>
     /// <typeparam name="TProperty">Type of object inside collection</typeparam>
     /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
     /// <param name="min"></param>
     /// <param name="max"></param>
     /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TProperty>> Count<T, TProperty>(
            this IRuleBuilder<T, OptionalCollection<TProperty>> ruleBuilder,
            int min,
            int max)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new CountValidator<TProperty>(min: min, max: max));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties Validation will fail if the count of the collection is
        /// outside of the specified range, The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TCollection">Type of object that is collection of TProperty</typeparam>
        /// <typeparam name="TProperty">Type of object inside collection</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TCollection>> Count<T, TCollection, TProperty>(
            this IRuleBuilder<T, Optional<TCollection>> ruleBuilder,
            int min,
            int max)
            where TCollection : ICollection<TProperty>
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new CountValidator<TProperty>(min: min, max: max));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// larger than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of object inside collection</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TProperty>> MaximumCount<T, TProperty>(
            this IRuleBuilder<T, OptionalCollection<TProperty>> ruleBuilder,
            int maximumCount)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new MaximumCountValidator<TProperty>(maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// larger than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of object inside collection</typeparam>
        /// <typeparam name="TCollection">Type of object that is collection of TProperty</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TCollection>> MaximumCount<T, TCollection, TProperty>(
            this IRuleBuilder<T, Optional<TCollection>> ruleBuilder,
            int maximumCount)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new MaximumCountValidator<TProperty>(maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// less than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of object inside collection</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, OptionalCollection<TProperty>> MinimumCount<T, TProperty>(
            this IRuleBuilder<T, OptionalCollection<TProperty>> ruleBuilder,
            int maximumCount)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new MinimumCountValidator<TProperty>(maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection{T}"/> properties. Validation will fail if the count of the collection is
        /// less than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of object inside collection</typeparam>
        /// <typeparam name="TCollection">Type of object that is collection of TProperty</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TCollection>> MinimumCount<T, TCollection, TProperty>(
            this IRuleBuilder<T, Optional<TCollection>> ruleBuilder,
            int maximumCount)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new MinimumCountValidator<TProperty>(maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see cref="ICollection"/>
        /// properties Validation will fail if the count of the collection is outside of the
        /// specified range, The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, ICollection> Count<T>(
            this IRuleBuilder<T, ICollection> ruleBuilder,
            int min,
            int max)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new CountValidator<T>(min: min, max: max));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection"/> properties. Validation will fail if the count of the collection is
        /// larger than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, ICollection> MaximumCount<T>(
            this IRuleBuilder<T, ICollection> ruleBuilder,
            int maximumCount)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new MaximumCountValidator<T>(maximumCount));
        }

        /// <summary>
        /// Defines a count validator on the current rule builder, but only for <see
        /// cref="ICollection"/> properties. Validation will fail if the count of the collection is less
        /// than the count specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumCount"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, ICollection> MinimumCount<T>(
            this IRuleBuilder<T, ICollection> ruleBuilder,
            int minimumCount)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new MinimumCountValidator<T>(minimumCount));
        }
    }
}