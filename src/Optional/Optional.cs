using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DeskDirector.Text.Json
{
    [Serializable, JsonConverter(typeof(OptionalConverter))]
    public readonly struct Optional<T> : IOptional<T>
    {
        public static readonly Optional<T> Undefined = new(OptionalState.Undefined);
        public static readonly Optional<T> Null = new(OptionalState.Null);

        public static Optional<T> WithValue([NotNull] T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new Optional<T>(value);
        }

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
            return Equals(other, EqualityComparer<T>.Default);
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

            return State switch {
                OptionalState.Undefined => OptionalState.Undefined.GetHashCode(),
                OptionalState.Null => OptionalState.Null.GetHashCode(),
                OptionalState.HasValue => value != null
                    ? HashCode.Combine(OptionalState.HasValue, value)
                    : throw new InvalidOperationException("State is HasValue, but value is NULL."),
                _ => throw new NotSupportedException($"State {State} is not supported.")
            };
        }

        public override string ToString()
        {
            T? value = Value;

            return State switch {
                OptionalState.Undefined => "undefined",
                OptionalState.Null => "null",
                OptionalState.HasValue => value?.ToString() ??
                                          throw new InvalidOperationException("State is HasValue, but value is NULL."),
                _ => throw new NotSupportedException($"State {State} is not supported.")
            };
        }
    }
}