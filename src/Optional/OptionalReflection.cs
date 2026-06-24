using System.Diagnostics.CodeAnalysis;

namespace DeskDirector.Text.Json
{
    public static class OptionalReflection
    {
        public static bool IsOptional(
            Type type,
            [NotNullWhen(true)] out Type? effectiveType)
        {
            ArgumentNullException.ThrowIfNull(type);

            effectiveType = null;

            if (!type.IsGenericType) {
                return false;
            }

            if (!IsCollection(type, out Type? childType)) {
                return IsSingle(type, out effectiveType);
            }

            effectiveType = childType.MakeArrayType();
            return true;
        }

        public static bool IsOptionalCollection(
            Type type,
            [NotNullWhen(true)] out Type? childType)
        {
            ArgumentNullException.ThrowIfNull(type);

            return IsCollection(type, out childType);
        }

        private static bool IsCollection(
            Type type,
            [NotNullWhen(true)] out Type? value)
        {
            value = null;

            if (!type.IsGenericType ||
                type.GetGenericTypeDefinition() != typeof(OptionalCollection<>)) {
                return false;
            }

            Type? itemType = type.GetGenericArguments().FirstOrDefault();
            if (itemType == null) {
                throw new InvalidOperationException("OptionalCollection<T> doesn't have GenericArguments");
            }

            if (typeof(IOptional).IsAssignableFrom(itemType)) {
                throw new InvalidOperationException("OptionalCollection<T>'s child type T is another optional type.");
            }

            value = itemType;
            return true;
        }

        private static bool IsSingle(
            Type type,
            [NotNullWhen(true)] out Type? value)
        {
            value = null;

            if (!type.IsGenericType ||
                type.GetGenericTypeDefinition() != typeof(Optional<>)) {
                return false;
            }

            Type? itemType = type.GetGenericArguments().FirstOrDefault();
            if (itemType == null) {
                throw new InvalidOperationException("Optional<T> doesn't have GenericArguments");
            }

            if (typeof(IOptional).IsAssignableFrom(itemType)) {
                throw new InvalidOperationException("Optional<T>'s child type T is another optional type.");
            }

            value = itemType;
            return true;
        }
    }
}