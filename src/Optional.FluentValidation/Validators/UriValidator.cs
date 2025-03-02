using FluentValidation;
using FluentValidation.Validators;

namespace DeskDirector.Text.Json.Validation.Validators
{
    public class UriValidator<TModel, TProperty> : PropertyValidator<TModel, TProperty>
    {
        public override string Name => "UriValidator";

        private readonly UriScheme _scheme;

        protected override string GetDefaultMessageTemplate(string errorCode) => ConstructErrorMessage(_scheme);

        private static string ConstructErrorMessage(UriScheme scheme)
        {
            return scheme.Is(UriScheme.None)
                ? "{PropertyName} is invalid URI"
                : $"{{PropertyName}} need to be valid URI with any of schemes in <{String.Join(", ", GetSchemeNames(scheme))}>";
        }

        public UriValidator(UriScheme scheme)
        {
            _scheme = scheme;
        }

        public override bool IsValid(ValidationContext<TModel> context, TProperty value)
        {
            return value switch {
                Optional<string> optional => IsValid(optional),
                string text => IsValid(text),
                _ => true
            };
        }

        private bool IsValid(Optional<string> optional)
        {
            return !optional.HasValue(out string? value) || IsValid(value);
        }

        private bool IsValid(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) {
                return false;
            }

            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute)) {
                return false;
            }

            return _scheme == UriScheme.None || HasCorrectScheme(value);
        }

        private bool HasCorrectScheme(string value)
        {
            return UriSchemeUtils.Checks.Any(c => c(_scheme, value));
        }

        private static IEnumerable<string> GetSchemeNames(UriScheme scheme)
        {
            if (scheme.Is(UriScheme.None)) {
                yield break;
            }

            if (scheme.Is(UriScheme.HTTP)) {
                yield return "http";
            }

            if (scheme.Is(UriScheme.HTTPS)) {
                yield return "https";
            }

            if (scheme.Is(UriScheme.FTP)) {
                yield return "ftp";
            }

            if (scheme.Is(UriScheme.MailTo)) {
                yield return "mailto";
            }

            if (scheme.Is(UriScheme.File)) {
                yield return "file";
            }

            if (scheme.Is(UriScheme.Data)) {
                yield return "data";
            }

            if (scheme.Is(UriScheme.WebSocket)) {
                yield return "ws";
            }

            if (scheme.Is(UriScheme.WebSocketSecure)) {
                yield return "wss";
            }
        }
    }

    public static class UriSchemeUtils
    {
        public static bool IsNot(this UriScheme source, UriScheme target)
        {
            if (target == UriScheme.None) {
                return source != UriScheme.None;
            }

            return (source & target) != target;
        }

        public static bool Is(this UriScheme source, UriScheme target)
        {
            if (target == UriScheme.None) {
                return source == UriScheme.None;
            }

            return (source & target) == target;
        }

        internal static readonly IReadOnlyCollection<Func<UriScheme, string, bool>> Checks = [
            EnsureHttp,
            EnsureHttps,
            EnsureData,
            EnsureFTP,
            EnsureFile,
            EnsureMailTo,
            EnsureWebSocket,
            EnsureWebSocketSecure
        ];

        private static bool EnsureHttp(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.HTTP) &&
                   value.AsSpan().TrimStart().StartsWith("HTTP://", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EnsureHttps(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.HTTPS) &&
                   value.AsSpan().TrimStart().StartsWith("HTTPS://", StringComparison.OrdinalIgnoreCase);
        }

        // ReSharper disable once InconsistentNaming
        private static bool EnsureFTP(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.FTP) &&
                   value.AsSpan().TrimStart().StartsWith("FTP://", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EnsureMailTo(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.MailTo) &&
                   value.AsSpan().TrimStart().StartsWith("mailto:", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EnsureFile(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.File) &&
                   value.AsSpan().TrimStart().StartsWith("file:", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EnsureData(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.Data) &&
                   value.AsSpan().TrimStart().StartsWith("data:", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EnsureWebSocket(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.WebSocket) &&
                   value.AsSpan().TrimStart().StartsWith("ws://", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EnsureWebSocketSecure(UriScheme scheme, string value)
        {
            return scheme.Is(UriScheme.WebSocketSecure) &&
                   value.AsSpan().TrimStart().StartsWith("wss://", StringComparison.OrdinalIgnoreCase);
        }
    }

    // ReSharper disable InconsistentNaming
    [Flags]
    public enum UriScheme
    {
        None = 0,
        HTTP = 1 << 0,
        HTTPS = 1 << 2,
        FTP = 1 << 3,
        MailTo = 1 << 4,
        File = 1 << 5,
        Data = 1 << 6,
        WebSocket = 1 << 7,
        WebSocketSecure = 1 << 8
    }
}