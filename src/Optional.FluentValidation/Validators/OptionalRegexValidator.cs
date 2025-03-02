using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class OptionalRegexValidator<TModel> : PropertyValidator<TModel, Optional<string>>
    {
        public override string Name => "OptionalRegexValidator";

        public string? Expression { get; }

        private readonly Func<TModel, Regex?> _regexFunc;

        public OptionalRegexValidator(
            [StringSyntax(StringSyntaxAttribute.Regex)] string expression,
            RegexOptions options = RegexOptions.None)
        {
            ArgumentException.ThrowIfNullOrEmpty(expression);

            Expression = expression;
            Regex? regex = CreateRegex(expression, options);
            _regexFunc = _ => regex;
        }

        public OptionalRegexValidator(Regex regex)
        {
            ArgumentNullException.ThrowIfNull(regex);

            Expression = regex.ToString();
            _regexFunc = _ => regex;
        }

        public OptionalRegexValidator(Func<TModel, string> expressionFunc, RegexOptions options = RegexOptions.None)
        {
            ArgumentNullException.ThrowIfNull(expressionFunc);

            _regexFunc = model => CreateRegex(expressionFunc(model), options);
        }

        public OptionalRegexValidator(Func<TModel, Regex?> regexFunc)
        {
            ArgumentNullException.ThrowIfNull(regexFunc);

            _regexFunc = regexFunc;
        }

        public override bool IsValid(ValidationContext<TModel> context, Optional<string> value)
        {
            if (!value.HasValue(out string? input)) {
                return true;
            }

            Regex? regex = _regexFunc(context.InstanceToValidate);
            if (regex != null && !regex.IsMatch(input)) {
                context.MessageFormatter.AppendArgument("RegularExpression", regex.ToString());
                return false;
            }

            return true;
        }

        private static Regex? CreateRegex(string expression, RegexOptions options = RegexOptions.None)
        {
            if (String.IsNullOrEmpty(expression)) {
                return null;
            }

            return new Regex(expression, options, TimeSpan.FromSeconds(2.0));
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return Localized(errorCode, "RegularExpressionValidator");
        }
    }
}