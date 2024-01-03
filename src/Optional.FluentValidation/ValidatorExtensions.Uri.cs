using DeskDirector.Text.Json.Validation.Validators;
using FluentValidation;

namespace DeskDirector.Text.Json.Validation
{
    public static partial class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, Optional<string>> Uri<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder,
            UriScheme scheme)
        {
            return ruleBuilder.SetValidator(new UriValidator<T, Optional<string>>(scheme));
        }

        public static IRuleBuilderOptions<T, string?> Uri<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            UriScheme scheme)
        {
            return ruleBuilder.SetValidator(new UriValidator<T, string?>(scheme));
        }

        public static IRuleBuilderOptions<T, Optional<string>> HttpUrl<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriValidator<T, Optional<string>>(UriScheme.HTTP | UriScheme.HTTPS));
        }

        public static IRuleBuilderOptions<T, string?> HttpUrl<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriValidator<T, string?>(UriScheme.HTTP | UriScheme.HTTPS));
        }

        public static IRuleBuilderOptions<T, Optional<string>> HttpsUrl<T>(
            this IRuleBuilder<T, Optional<string>> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriValidator<T, Optional<string>>(UriScheme.HTTPS));
        }

        public static IRuleBuilderOptions<T, string?> HttpsUrl<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriValidator<T, string?>(UriScheme.HTTPS));
        }
    }
}