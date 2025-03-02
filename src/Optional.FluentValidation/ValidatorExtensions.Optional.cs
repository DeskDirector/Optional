using DeskDirector.Text.Json.Validation.Validators;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Associates an instance of IValidator with the current property rule, IValidator will only
        /// be used when Optional{T} has value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="validator">validator</param>
        /// <param name="ruleSets">rule sets</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, Optional<TProperty>> SetValidator<T, TProperty>(
            this IRuleBuilder<T, Optional<TProperty>> ruleBuilder,
            IValidator<TProperty> validator,
            params string[] ruleSets)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentNullException.ThrowIfNull(validator);

            OptionalValidator<TProperty> optional = new(validator);
            return ruleBuilder.SetValidator(optional, ruleSets);
        }
    }
}