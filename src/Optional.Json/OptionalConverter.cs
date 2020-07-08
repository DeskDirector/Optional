using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nness.Text.Json
{
    public class OptionalConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType) {
                return false;
            }

            return typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert.GetGenericArguments().Length > 0);

            Type type = typeToConvert.GetGenericArguments()[0];

            object? converterObj = Activator.CreateInstance(
                typeof(OptionalConverterInner<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { options },
                null
            );
            if (converterObj == null) {
                throw new InvalidOperationException($"Unable to to create converter for Optional<{type.Name}>");
            }

            var converter = (JsonConverter)converterObj;
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

                TValue value = _valueConverter == null
                    ? JsonSerializer.Deserialize<TValue>(ref reader, options)
                    : _valueConverter.Read(ref reader, _valueType, options);
                return new Optional<TValue>(value);
            }

            public override void Write(Utf8JsonWriter writer, Optional<TValue> value, JsonSerializerOptions options)
            {
                if (value.IsUndefined() || value.IsNull()) {
                    writer.WriteNullValue();
                    return;
                }

                TValue data = value.Value;
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