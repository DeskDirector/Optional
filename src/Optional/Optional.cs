using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Nness.Text.Json
{
    [Serializable]
    public struct Optional<T> : IOptional<T>
    {
        public static readonly Optional<T> Undefined = new Optional<T>(OptionalState.Undefined);
        public static readonly Optional<T> Null = new Optional<T>(OptionalState.Null);

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
            value = _value;
            return State == OptionalState.HasValue;
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

        public bool IsSet() => State != OptionalState.Undefined;

        public bool IsNull() => State == OptionalState.Null;

        public bool IsUndefined() => State == OptionalState.Undefined;

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public override bool Equals(object? obj)
        {
            switch (obj) {
                case null:
                    return State == OptionalState.Null;

                case Optional<T> other:
                    return Equals(other);

                case T value:
                    return Equals(new Optional<T>(value));

                default:
                    return false;
            }
        }

        public bool Equals(Optional<T> other)
        {
            if (State != other.State) {
                return false;
            }

            if (!HasValue(out T current)) {
                return true;
            }

            other.HasValue(out T otherValue);

            return current.Equals(otherValue);
        }

        public bool Equals(Optional<T> other, EqualityComparer<T> comparer)
        {
            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (State != other.State) {
                return false;
            }

            if (!HasValue(out T current)) {
                return true;
            }

            other.HasValue(out T otherValue);

            return comparer.Equals(current, otherValue);
        }

        public override int GetHashCode()
        {
            if (HasValue(out T value)) {
                return value.GetHashCode();
            }

            switch (State) {
                case OptionalState.Undefined:
                    return -1;

                case OptionalState.Null:
                    return 0;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            if (HasValue(out T value)) {
                return value.ToString() ?? String.Empty;
            }

            switch (State) {
                case OptionalState.Undefined:
                    return "undefined";

                case OptionalState.Null:
                    return "null";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}