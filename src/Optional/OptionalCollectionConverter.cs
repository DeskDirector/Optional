using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeskDirector.Text.Json
{
    public class OptionalCollectionConverter : JsonConverterFactory
    {
        /// <param name="type"><see cref="OptionalCollection{T}"/> type</param>
        /// <returns>Is <see cref="Type"/> <see cref="OptionalCollection{T}"/></returns>
        public static bool IsOptional(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(OptionalCollection<>);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return IsOptional(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert.GetGenericArguments().Length > 0);

            Type type = typeToConvert.GetGenericArguments()[0];

            object? converterObj = Activator.CreateInstance(
                typeof(OptionalCollectionConverterInner<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { options },
                null
            );
            if (converterObj == null) {
                throw new InvalidOperationException(
                    $"Unable to to create converter for OptionalCollection<{type.Name}>"
                );
            }

            JsonConverter converter = (JsonConverter)converterObj;
            return converter;
        }

        private class OptionalCollectionConverterInner<TValue> : JsonConverter<OptionalCollection<TValue>>
        {
            private readonly JsonConverter<ICollection<TValue>>? _valueConverter;
            private readonly Type _valueType;

            public OptionalCollectionConverterInner(JsonSerializerOptions options)
            {
                _valueConverter = (JsonConverter<ICollection<TValue>>)options.GetConverter(typeof(ICollection<TValue>));
                _valueType = typeof(ICollection<TValue>);
            }

            public override OptionalCollection<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null) {
                    return new OptionalCollection<TValue>(OptionalState.Null);
                }

                if (reader.TokenType != JsonTokenType.StartArray) {
                    throw new InvalidOperationException(
                        $"Deserialize encounter unexpected value, value is not collection. type<{reader.TokenType}>"
                    );
                }

                ICollection<TValue>? value = _valueConverter == null
                    ? JsonSerializer.Deserialize<TValue[]>(ref reader, options)
                    : _valueConverter.Read(ref reader, _valueType, options);
                return new OptionalCollection<TValue>(value ?? Array.Empty<TValue>());
            }

            public override void Write(Utf8JsonWriter writer, OptionalCollection<TValue> value, JsonSerializerOptions options)
            {
                if (value.IsUndefined()) {
                    throw new InvalidOperationException(
                        "Value is undefined, it cannot be serialized. Seek for TypeInfoResolver."
                    );
                }

                if (value.IsNull()) {
                    writer.WriteNullValue();
                    return;
                }

                ICollection<TValue>? data = value.Value;
                if (data == null) {
                    throw new InvalidOperationException("Optional return Null value while the state is HasValue");
                }

                if (_valueConverter == null) {
                    JsonSerializer.Serialize(writer, data, options);
                } else {
                    _valueConverter.Write(writer, data, options);
                }
            }
        }
    }
}