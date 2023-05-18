using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Nness.Text.Json
{
    [Serializable, JsonConverter(typeof(OptionalConverter))]
    public readonly struct Optional<T> : IOptional<T>
    {
        public static readonly Optional<T> Undefined = new(OptionalState.Undefined);
        public static readonly Optional<T> Null = new(OptionalState.Null);

        private readonly T? _value;

        public OptionalState State { get; }

        public T? Value => State == OptionalState.HasValue && _value != null ? _value : default;

        public Optional()
        {
            _value = default;
            State = OptionalState.Undefined;
        }

        public Optional(T? value)
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

        public bool HasValue([NotNullWhen(true)] out T? value)
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

        public bool IsSet(out T? value)
        {
            value = default;

            switch (State) {
                case OptionalState.Null:
                    return true;

                case OptionalState.HasValue:
                    value = _value;
                    return true;

                case OptionalState.Undefined:
                default:
                    return false;
            }
        }

        public bool IsNull() => State == OptionalState.Null;

        public bool IsUndefined() => State == OptionalState.Undefined;

        public static implicit operator Optional<T>(T? value)
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

            T? current = Value;
            T? otherValue = other.Value;

            if (ReferenceEquals(current, otherValue)) {
                return true;
            }

            if (current == null || otherValue == null) {
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return comparer.Equals(current, otherValue);
        }

        public bool Equals(Optional<T> other, IEqualityComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);

            if (State != other.State) {
                return false;
            }

            T? current = Value;
            T? otherValue = other.Value;

            if (ReferenceEquals(current, otherValue)) {
                return true;
            }

            if (current == null || otherValue == null) {
                return false;
            }

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
            T? value = Value;
            if (value != null) {
                return value.GetHashCode();
            }

            return State switch {
                OptionalState.Undefined => -1,
                OptionalState.Null => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override string ToString()
        {
            T? value = Value;
            if (value != null) {
                return value.ToString() ?? String.Empty;
            }

            return State switch {
                OptionalState.Undefined => "undefined",
                OptionalState.Null => "null",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}