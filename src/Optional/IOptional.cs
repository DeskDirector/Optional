using System;
using System.Diagnostics.CodeAnalysis;

namespace Nness.Optional
{
    public interface IOptional
    {
        bool IsNull();

        bool IsUndefined();

        OptionalState State { get; }

        bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out object? value);
    }

    public interface IOptional<T> : IOptional
    {
        bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out T value);
    }
}