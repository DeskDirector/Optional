using System.Diagnostics.CodeAnalysis;

namespace DeskDirector.Text.Json
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

        bool HasValue([NotNullWhen(true)] out object? value);
    }

    public interface IOptional<T> : IOptional
    {
        bool IsSet(out T? value);

        bool HasValue([NotNullWhen(true)] out T? value);

        /// <summary>
        /// Return value when value been set, else it should return default value of <see cref="T"/>
        /// </summary>
        T? Value { get; }
    }
}