using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeskDirector.Text.Json
{
    public class OptionalConverter : JsonConverterFactory
    {
        /// <param name="type"><see cref="Optional{T}"/> type</param>
        /// <returns>Is <see cref="Type"/> <see cref="Optional{T}"/>.</returns>
        public static bool IsOptional(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>);
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
                typeof(OptionalConverterInner<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                [options],
                null
            );
            if (converterObj == null) {
                throw new InvalidOperationException($"Unable to to create converter for Optional<{type.Name}>");
            }

            JsonConverter converter = (JsonConverter)converterObj;
            return converter;
        }

        private class OptionalConverterInner<TValue> : JsonConverter<Optional<TValue>>
        {
            private readonly JsonConverter<TValue>? _valueConverter;
            private readonly Type _valueType;

            public OptionalConverterInner(JsonSerializerOptions options)
            {
                _valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
                _valueType = typeof(TValue);
            }

            public override Optional<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null) {
                    return new Optional<TValue>(OptionalState.Null);
                }

                TValue? value = _valueConverter == null
                    ? JsonSerializer.Deserialize<TValue>(ref reader, options)
                    : _valueConverter.Read(ref reader, _valueType, options);
                return new Optional<TValue>(value);
            }

            public override void Write(Utf8JsonWriter writer, Optional<TValue> value, JsonSerializerOptions options)
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

                TValue? data = value.Value;
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