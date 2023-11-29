using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nness.Text.Json.NewtonsoftJson
{
    public class OptionalCollectionJsonConverter : JsonConverter
    {
        public static readonly OptionalCollectionJsonConverter Default = new();

        private static readonly JToken NullToken = new JValue((object?)null);

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) {
                NullToken.WriteTo(writer);
                return;
            }

            if (value is not IOptional optional) {
                throw new InvalidOperationException("Value is not optional");
            }

            if (!optional.CanSerialize(serializer, out JToken? token)) {
                throw new InvalidOperationException(
                    "Optional value can only be written in JSON if it is not undefined."
                );
            }

            token.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            IOptionalTypedConverter converter = OptionalCollectionTypedConverter.GetConverter(objectType);

            JToken obj = JToken.Load(reader);
            if (obj.Type == JTokenType.Null) {
                return converter.Null;
            }

            return converter.Value(obj);
        }

        public override bool CanConvert(Type objectType)
        {
            return OptionalCollectionTypedConverter.IsOptionalType(objectType);
        }
    }
}