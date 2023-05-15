using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Nness.Text.Json
{
    [Serializable, JsonConverter(typeof(OptionalConverter))]
    public readonly struct Optional<T> : IOptional<T>
    {
        public static readonly Optional<T> Undefined = new Optional<T>(OptionalState.Undefined);
        public static readonly Optional<T> Null = new Optional<T>(OptionalState.Null);

        [AllowNull, MaybeNull]
        private readonly T _value;

        public OptionalState State { get; }

        [MaybeNull]
        public T Value
        {
            get {
                if (State == OptionalState.HasValue && _value != null) {
                    return _value;
                }
                return default!;
            }
        }

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
            _value = default;
            State = state;
        }

        public bool HasValue([NotNullWhen(true), MaybeNullWhen(false)] out T value)
        {
            value = _value;
            return value != null && State == OptionalState.HasValue;
        }

        public bool HasValue([NotNullWhen(true)] out object? value)
        {
            value = _value;
            return value != null && State == OptionalState.HasValue;
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

            T current = Value;
            T otherValue = other.Value;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return comparer.Equals(current, otherValue);
        }

        public bool Equals(Optional<T> other, IEqualityComparer<T> comparer)
        {
            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (State != other.State) {
                return false;
            }

            T current = Value;
            T otherValue = other.Value;

            return comparer.Equals(current, otherValue);
        }

        public static bool operator ==(Optional<T> item1, Optional<T> item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Optional<T> item1, Optional<T> item2)
        {
            return !item1.Equals(item2);
        }

        public override int GetHashCode()
        {
            T value = Value;
            if (value != null) {
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
            T value = Value;
            if (value != null) {
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