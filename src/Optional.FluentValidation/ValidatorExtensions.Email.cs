using DeskDirector.Text.Json.Validation.Validators;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Email validator without using Regex
        /// </summary>
        public static IRuleBuilderOptions<T, Optional<string>> Email<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);

            return ruleBuilder.SetValidator(new EmailValidator<T>());
        }
    }
}