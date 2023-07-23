using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Nness.Text.Json;

namespace Optional.NewtonsoftJson
{
    public class OptionalCollectionTypedConverter
    {
        private static readonly ConcurrentDictionary<Type, IOptionalTypedConverter> Converters = new();

        public static IOptionalTypedConverter GetConverter(Type objectType)
        {
            ArgumentNullException.ThrowIfNull(objectType);

            if (Converters.TryGetValue(objectType, out IOptionalTypedConverter? converter)) {
                return converter;
            }

            if (objectType == typeof(IOptional)) {
                throw new InvalidOperationException(
                    "Collection typed convert is unable to convert IOptional, please use OptionalCollection<T>."
                );
            }

            Type valueType = ResolveOptionalTypeParameter(objectType);

            object? instance = Activator.CreateInstance(
                typeof(OptionalCollectionTypedConverter<>).MakeGenericType(valueType)
            );

            if (instance is not IOptionalTypedConverter typedConverter) {
                throw new InvalidOperationException(
                    $"Failed initialize {nameof(OptionalCollectionTypedConverter)}<{valueType.Name}>"
                );
            }

            converter = typedConverter;
            return Converters.GetOrAdd(objectType, converter);
        }

        public static Type ResolveOptionalTypeParameter(Type optionalType)
        {
            if (TryResolveOptionalTypeParameter(optionalType, out Type? valueType)) {
                return valueType;
            }

            throw new InvalidOperationException($"Type ({optionalType.FullName}) is not OptionalCollection<T>.");
        }

        public static bool TryResolveOptionalTypeParameter(
            Type optionalType,
            [NotNullWhen(true)] out Type? valueType)
        {
            ArgumentNullException.ThrowIfNull(optionalType);

            valueType = default;
            if (!optionalType.IsGenericType ||
                optionalType.GetGenericTypeDefinition() != typeof(OptionalCollection<>)) {
                return false;
            }

            Type itemType = optionalType.GetGenericArguments()[0];
            if (typeof(IOptional).IsAssignableFrom(itemType)) {
                throw new InvalidOperationException("OptionalCollection's child type cannot be Optional.");
            }

            valueType = itemType;
            return true;
        }
    }
}