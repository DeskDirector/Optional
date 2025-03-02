using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentValidation;
using DeskDirector.Text.Json.Validation.Validators;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, Optional<string>> Matches<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            [StringSyntax(StringSyntaxAttribute.Regex)] string expression,
            RegexOptions options = RegexOptions.None)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentException.ThrowIfNullOrEmpty(expression);

            return ruleBuilder.SetValidator(new OptionalRegexValidator<T>(expression, options));
        }

        public static IRuleBuilderOptions<T, Optional<string>> Matches<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            Func<T, string> expression,
            RegexOptions options = RegexOptions.None)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentNullException.ThrowIfNull(expression);

            return ruleBuilder.SetValidator(new OptionalRegexValidator<T>(expression, options));
        }

        public static IRuleBuilderOptions<T, Optional<string>> Matches<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            Regex regex)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentNullException.ThrowIfNull(regex);

            return ruleBuilder.SetValidator(new OptionalRegexValidator<T>(regex));
        }

        public static IRuleBuilderOptions<T, Optional<string>> Matches<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            Func<T, Regex> regex)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder);
            ArgumentNullException.ThrowIfNull(regex);

            return ruleBuilder.SetValidator(new OptionalRegexValidator<T>(regex));
        }
    }
}