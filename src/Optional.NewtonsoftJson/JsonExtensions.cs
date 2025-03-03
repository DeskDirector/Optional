﻿using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeskDirector.Text.Json.NewtonsoftJson
{
    public static class JsonExtensions
    {
        public static bool CanSerialize(
            this IOptional value,
            JsonSerializer serializer,
            [NotNullWhen(true)] out JToken? token)
        {
            ArgumentNullException.ThrowIfNull(value);

            token = null;

            if (value.HasValue(out object? model)) {
                token = JToken.FromObject(model, serializer);
                return true;
            }

            if (value.IsNull()) {
                token = new JValue((object?)null);
                return true;
            }

            if (value.IsUndefined()) {
                return false;
            }

            throw new InvalidOperationException(
                $"Optional with state {value.State} is either not supported or it doesn't contain value."
            );
        }

        public static JsonSerializerSettings AppendIOptionalConverters(
            this JsonSerializerSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            settings.Converters.Add(new OptionalJsonConverter());
            settings.Converters.Add(new OptionalCollectionJsonConverter());

            return settings;
        }
    }
}