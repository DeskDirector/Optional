﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Nness.Text.Json
{
    public class OptionalCollection<T> : IOptional<ICollection<T>>, IEnumerable<T>
    {
        [AllowNull]
        private readonly ICollection<T> _value;

        public OptionalState State { get; }

        public OptionalCollection(ICollection<T> value)
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

        public bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out ICollection<T> value)
        {
            value = _value;
            return State == OptionalState.HasValue;
        }

        public bool HasValue([NotNullWhen(true), MaybeNullWhen(false)]out object? value)
        {
            if (HasValue(out ICollection<T>? items)) {
                value = items;
                return true;
            }

            value = default;
            return false;
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
            if (HasValue(out ICollection<T>? value)) {
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
            if (HasValue(out ICollection<T>? value)) {
                // ReSharper disable once ConstantNullCoalescingCondition
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