#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Nness.Text.Json
{
    public class OptionalJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        public static readonly OptionalJsonTypeInfoResolver Default = new();

        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);
            if (typeof(IOptional).IsAssignableFrom(type) || jsonTypeInfo.Kind != JsonTypeInfoKind.Object || jsonTypeInfo.Properties.Count == 0) {
                return jsonTypeInfo;
            }

            foreach (JsonPropertyInfo propertyInfo in jsonTypeInfo.Properties) {
                AdjustForIOptional(propertyInfo);
            }

            return jsonTypeInfo;
        }

        private static void AdjustForIOptional(JsonPropertyInfo propertyInfo)
        {
            Type type = propertyInfo.PropertyType;
            if (!typeof(IOptional).IsAssignableFrom(type)) {
                return;
            }

            propertyInfo.ShouldSerialize = ShouldSerialize;
        }

        public static bool ShouldSerialize(object parent, object? current)
        {
            if (current is IOptional optional) {
                return !optional.IsUndefined();
            }

            return true;
        }
    }
}