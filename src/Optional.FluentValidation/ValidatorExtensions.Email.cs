using System;
using FluentValidation;
using Nness.Text.Json.Validation.Validators;

namespace Nness.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Email validator without using Regex
        /// </summary>
        public static IRuleBuilderOptions<T, string> Email<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new EmailValidator());
        }

        /// <summary>
        /// Email validator without using Regex
        /// </summary>
        public static IRuleBuilderOptions<T, Optional<string>> Email<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder)
        {
            if (ruleBuilder == null) {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new EmailValidator());
        }
    }
}