using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Nness.Text.Json.Validation.Validators
{
    public static class ValidatorHelper
    {
        private static readonly ConcurrentDictionary<Type, string> ResolvedErrorCodes =
            new ConcurrentDictionary<Type, string>();

        public static string ResolveErrorCode(PropertyValidator validator)
        {
            if (validator == null) {
                throw new ArgumentNullException(nameof(validator));
            }

            Type type = validator.GetType();
            return ToErrorCode(type);
        }

        public static string ToErrorCode([NotNull] Type type)
        {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }

            if (ResolvedErrorCodes.TryGetValue(type, out string? errorCode)) {
                return errorCode;
            }

            ReadOnlySpan<char> name = type.Name.AsSpan();
            int validatorIndex = name.IndexOf("Validator", StringComparison.OrdinalIgnoreCase);
            errorCode = validatorIndex <= 0 ? type.Name : name.Slice(0, validatorIndex).ToString();

            return ResolvedErrorCodes.GetOrAdd(type, errorCode);
        }

        public static IStringSource ErrorCodeSource([NotNull] Type type)
        {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }

            return new StaticStringSource(ToErrorCode(type));
        }
    }
}