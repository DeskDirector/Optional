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
                : $"{{PropertyName}} need to be valid URI with any of schemes in <{String.Join(", ", scheme.GetNames())}>";
        }

        public UriValidator(UriScheme scheme)
        {
            _scheme = scheme;
        }

        public override bool IsValid(ValidationContext<TModel> context, TProperty value)
        {
            return value switch {
                Optional<string> optional => IsValid(optional),
                string text => _scheme.IsValid(text),
                _ => true
            };
        }

        private bool IsValid(Optional<string> optional)
        {
            return !optional.HasValue(out string? value) || _scheme.IsValid(value);
        }
    }

    public static class UriSchemeUtils
    {
        extension(UriScheme source)
        {
            public bool IsNot(UriScheme target)
            {
                if (target == UriScheme.None) {
                    return source != UriScheme.None;
                }

                return (source & target) != target;
            }

            public bool Is(UriScheme target)
            {
                if (target == UriScheme.None) {
                    return source == UriScheme.None;
                }

                return (source & target) == target;
            }

            public bool IsValid(string? value)
            {
                if (String.IsNullOrWhiteSpace(value)) {
                    return false;
                }

                if (!Uri.IsWellFormedUriString(value, UriKind.Absolute)) {
                    return false;
                }

                return source == UriScheme.None || Checks.Any(c => c(source, value));
            }

            public IEnumerable<string> GetNames()
            {
                if (source.Is(UriScheme.None)) {
                    yield break;
                }

                if (source.Is(UriScheme.HTTP)) {
                    yield return "http";
                }

                if (source.Is(UriScheme.HTTPS)) {
                    yield return "https";
                }

                if (source.Is(UriScheme.FTP)) {
                    yield return "ftp";
                }

                if (source.Is(UriScheme.MailTo)) {
                    yield return "mailto";
                }

                if (source.Is(UriScheme.File)) {
                    yield return "file";
                }

                if (source.Is(UriScheme.Data)) {
                    yield return "data";
                }

                if (source.Is(UriScheme.WebSocket)) {
                    yield return "ws";
                }

                if (source.Is(UriScheme.WebSocketSecure)) {
                    yield return "wss";
                }
            }
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