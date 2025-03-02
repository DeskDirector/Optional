using System.Collections.Frozen;
using DeskDirector.Text.Json.Validation.Validators;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Defines a set validator on the current rule builder. Validation will fail is item is not
        /// part of valid set. Null value won't fail, use NotNull alongside with this.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="validSet"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> In<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder,
            FrozenSet<TProperty> validSet)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            if (validSet == null) {
                throw new ArgumentNullException(nameof(validSet));
            }

            return ruleBuilder.SetValidator(new InValidator<T, Optional<TProperty>, TProperty>(validSet));
        }

        public static IRuleBuilderOptions<T, TProperty?> In<T, TProperty>(
            this IRuleBuilder<T, TProperty?> ruleBuilder,
             FrozenSet<TProperty> validSet)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            if (validSet == null) {
                throw new ArgumentNullException(nameof(validSet));
            }

            return ruleBuilder.SetValidator(new InValidator<T, TProperty?, TProperty>(validSet));
        }

        public static IRuleBuilderOptions<T, TProperty?> In<T, TProperty>(
            this IRuleBuilder<T, TProperty?> ruleBuilder,
            FrozenSet<TProperty> validSet)
            where TProperty : struct
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            if (validSet == null) {
                throw new ArgumentNullException(nameof(validSet));
            }

            return ruleBuilder.SetValidator(new InValidator<T, TProperty?, TProperty>(validSet));
        }
    }
}