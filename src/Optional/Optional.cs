using System;
using System.Diagnostics.CodeAnalysis;

namespace Nness.Optional
{
    public class Optional<T> : IOptional<T>
    {
        private readonly T _value;

        public OptionalState State { get; }

        public Optional(T value)
        {
            _value = value;
            State = value == null ? OptionalState.Null : OptionalState.HasValue;
        }

        public Optional(OptionalState state)
        {
            if (state == OptionalState.HasValue) {
                throw new ArgumentException("State cannot be HasValue when value not been provided", nameof(state));
            }
            _value = default!;
            State = state;
        }

        public bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out T value)
        {
            value = default!;
            if (State != OptionalState.HasValue) {
                return false;
            }

            value = _value;
            return true;
        }

        public bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out object? value)
        {
            if (HasValue(out T item)) {
                value = item;
                return true;
            }

            value = default;
            return false;
        }

        public bool IsNull() => State == OptionalState.Null;

        public bool IsUndefined() => State == OptionalState.Undefined;
    }
}