using System.Diagnostics.CodeAnalysis;

namespace Nness.Optional
{
    public interface IOptional
    {
        /// <summary>
        /// When value is not undefined
        /// </summary>
        bool IsSet();

        /// <summary>
        /// When value is null
        /// </summary>
        bool IsNull();

        /// <summary>
        /// When value is undefined
        /// </summary>
        bool IsUndefined();

        OptionalState State { get; }

        bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out object? value);
    }

    public interface IOptional<T> : IOptional
    {
        bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out T value);
    }
}