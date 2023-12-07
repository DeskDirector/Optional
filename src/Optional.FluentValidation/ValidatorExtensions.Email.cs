using System;
using FluentValidation;
using DeskDirector.Text.Json.Validation.Validators;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Email validator without using Regex
        /// </summary>
        public static IRuleBuilderOptions<T, IOptional<string>> Email<T>(
            this IRuleBuilder<T, IOptional<string>> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new EmailValidator<T>());
        }
    }
}