using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeskDirector.Text.Json.NewtonsoftJson
{
    public class OptionalContractResolver : DefaultContractResolver
    {
        /// <param name="namingStrategy">Default to <see cref="CamelCaseNamingStrategy"/></param>
        public OptionalContractResolver(NamingStrategy? namingStrategy = null)
        {
            NamingStrategy = namingStrategy ?? new CamelCaseNamingStrategy();
        }

        public OptionalContractResolver(IContractResolver? resolver)
        {
            if (resolver is not DefaultContractResolver defaultResolver) {
                NamingStrategy = new CamelCaseNamingStrategy();
                return;
            }

            NamingStrategy = defaultResolver.NamingStrategy;
            SerializeCompilerGeneratedMembers = defaultResolver.SerializeCompilerGeneratedMembers;
            IgnoreSerializableInterface = defaultResolver.IgnoreSerializableInterface;
            IgnoreSerializableAttribute = defaultResolver.IgnoreSerializableAttribute;
            IgnoreIsSpecifiedMembers = defaultResolver.IgnoreIsSpecifiedMembers;
            IgnoreShouldSerializeMembers = defaultResolver.IgnoreShouldSerializeMembers;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            bool _ = TryAdjustForIOptional(property);
            return property;
        }

        public static bool TryAdjustForIOptional(JsonProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (!HasOptionalInterface(property.PropertyType)) {
                return false;
            }

            property.ShouldSerialize = ShouldSerialize(property);

            AssignOptionalConverter(property);

            if (IsOptionalType(property.PropertyType)) {
                return true;
            }

            property.ShouldDeserialize = _ => false;
            return true;
        }

        public static void AssignOptionalConverter(JsonProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (property.Converter != null || property.PropertyType == null) {
                return;
            }

            if (OptionalTypedConverter.IsOptionalType(property.PropertyType)) {
                property.Converter = OptionalJsonConverter.Default;
            } else if (OptionalCollectionTypedConverter.IsOptionalType(property.PropertyType)) {
                property.Converter = OptionalCollectionJsonConverter.Default;
            }
        }

        private static bool HasOptionalInterface([NotNullWhen(true)] Type? type)
        {
            return type != null && typeof(IOptional).IsAssignableFrom(type);
        }

        private static bool IsOptionalType(Type type)
        {
            return OptionalTypedConverter.IsOptionalType(type) ||
                   OptionalCollectionTypedConverter.IsOptionalType(type);
        }

        private static Predicate<object> ShouldSerialize(JsonProperty property)
        {
            return obj => ShouldSerialize(property, obj);
        }

        private static bool ShouldSerialize(JsonProperty property, object value)
        {
            object? obj = property.ValueProvider?.GetValue(value);
            if (obj is not IOptional optional) {
                throw new InvalidOperationException(
                    $"Type ({property.PropertyType?.FullName}) is not associated with Optional<T> or OptionalCollection<T>."
                );
            }

            bool isUndefined = optional.IsUndefined();
            return !isUndefined;
        }
    }
}