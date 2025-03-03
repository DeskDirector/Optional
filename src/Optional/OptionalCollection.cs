﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DeskDirector.Text.Json
{
    [Serializable, JsonConverter(typeof(OptionalCollectionConverter))]
    public readonly struct OptionalCollection<T> : IOptional<IReadOnlyCollection<T>>, IEnumerable<T>
    {
        public static readonly OptionalCollection<T> Undefined = new(OptionalState.Undefined);

        public static readonly OptionalCollection<T> Null = new(OptionalState.Null);

        public static OptionalCollection<T> WithValue(IReadOnlyCollection<T> value)
        {
            ArgumentNullException.ThrowIfNull(value);

            return new OptionalCollection<T>(value);
        }

        private readonly IReadOnlyCollection<T>? _value;

        public OptionalState State { get; }

        public IReadOnlyCollection<T>? Value => State == OptionalState.HasValue && _value != null ? _value : null;

        public OptionalCollection(IReadOnlyCollection<T>? value)
        {
            _value = value;
            State = value == null ? OptionalState.Null : OptionalState.HasValue;
        }

        public OptionalCollection(OptionalState state)
        {
            if (state == OptionalState.HasValue) {
                throw new ArgumentException("State cannot be HasValue when value not been provided", nameof(state));
            }
            _value = null;
            State = state;
        }

        public bool IsSet() => State != OptionalState.Undefined;

        public bool IsSet(out IReadOnlyCollection<T>? value)
        {
            value = null;

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

        public bool HasValue([NotNullWhen(true)] out IReadOnlyCollection<T>? value)
        {
            value = _value;
            return value != null && State == OptionalState.HasValue;
        }

        public bool HasValue([NotNullWhen(true)] out object? value)
        {
            value = _value;
            return value != null && State == OptionalState.HasValue;
        }

        public bool IsNull() => State == OptionalState.Null;

        public bool IsUndefined() => State == OptionalState.Undefined;

        public IEnumerator<T> GetEnumerator()
        {
            return _value == null
                ? Enumerable.Empty<T>().GetEnumerator()
                : _value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override int GetHashCode()
        {
            IReadOnlyCollection<T>? value = Value;

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
            IReadOnlyCollection<T>? value = Value;

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