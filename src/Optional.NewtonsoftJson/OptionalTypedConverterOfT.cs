using Newtonsoft.Json.Linq;
using Nness.Text.Json;

namespace Optional.NewtonsoftJson
{
    public class OptionalTypedConverter<T> : IOptionalTypedConverter
    {
        public IOptional Null => Optional<T>.Null;

        public IOptional Undefined => Optional<T>.Undefined;

        public IOptional Value(JToken? token)
        {
            if (token == null) {
                return Null;
            }

            if (token.Type == JTokenType.Null) {
                return Null;
            }

            T? value = token.ToObject<T>();
            if (value == null) {
                throw new InvalidOperationException(
                    $"token type is ({token.Type}) but unable convert to {typeof(T).Name}"
                );
            }

            return new Optional<T>(value);
        }
    }
}