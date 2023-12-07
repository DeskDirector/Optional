using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace DeskDirector.Text.Json.NewtonsoftJson
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

            if (!IsOptionalType(objectType)) {
                throw new ArgumentException(
                    $"Type ({objectType.FullName}) is not OptionalCollection<T>.",
                    nameof(objectType)
                );
            }

            Type valueType = ResolveOptionalTypeParameter(objectType);

            object? instance = Activator.CreateInstance(
                typeof(OptionalCollectionTypedConverter<>).MakeGenericType(valueType)
            );

            if (instance is not IOptionalTypedConverter typedConverter) {
                throw new InvalidOperationException(
                    $"Failed initialize to {nameof(OptionalCollectionTypedConverter)}<{valueType.Name}>"
                );
            }

            converter = typedConverter;
            return Converters.GetOrAdd(objectType, converter);
        }

        private static Type ResolveOptionalTypeParameter(Type optionalType)
        {
            if (TryResolveOptionalTypeParameter(optionalType, out Type? valueType)) {
                return valueType;
            }

            throw new ArgumentException(
                $"Type ({optionalType.FullName}) is not OptionalCollection<T>.",
                nameof(optionalType)
            );
        }

        public static bool TryResolveOptionalTypeParameter(
            Type optionalType,
            [NotNullWhen(true)] out Type? valueType)
        {
            ArgumentNullException.ThrowIfNull(optionalType);

            valueType = default;
            if (!IsOptionalType(optionalType)) {
                return false;
            }

            Type itemType = optionalType.GetGenericArguments().First();
            if (typeof(IOptional).IsAssignableFrom(itemType)) {
                throw new InvalidOperationException("OptionalCollection's child type cannot be Optional.");
            }

            valueType = itemType;
            return true;
        }

        public static bool IsOptionalType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            return OptionalCollectionConverter.IsOptional(type);
        }
    }
}