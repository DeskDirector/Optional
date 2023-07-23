using Newtonsoft.Json.Linq;
using Nness.Text.Json;

namespace Optional.NewtonsoftJson
{
    public class OptionalCollectionTypedConverter<T> : IOptionalTypedConverter
    {
        public IOptional Null => OptionalCollection<T>.Null;

        public IOptional Undefined => OptionalCollection<T>.Undefined;

        public IOptional Value(JToken? token)
        {
            if (token == null) {
                return Null;
            }

            if (token.Type == JTokenType.Null) {
                return Null;
            }

            ICollection<T>? value = token.ToObject<ICollection<T>>();
            if (value == null) {
                throw new InvalidOperationException(
                    $"token type is ({token.Type}) but unable convert to ICollection<{typeof(T).Name}>"
                );
            }

            return new OptionalCollection<T>(value);
        }
    }
}