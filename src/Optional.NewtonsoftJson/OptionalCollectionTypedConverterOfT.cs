using Newtonsoft.Json.Linq;

namespace DeskDirector.Text.Json.NewtonsoftJson
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

            IReadOnlyCollection<T>? value = token.ToObject<IReadOnlyCollection<T>>();
            if (value == null) {
                throw new InvalidOperationException(
                    $"token type is ({token.Type}) but unable convert to ICollection<{typeof(T).Name}>"
                );
            }

            return new OptionalCollection<T>(value);
        }
    }
}