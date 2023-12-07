using System;
using System.Collections.Generic;
using Xunit;

namespace DeskDirector.Text.Json.Tests
{
    public class OptionalTests
    {
        [Theory]
        [MemberData(nameof(ValueTypeHasValueSamples))]
        public void ValueTypeHasValue(Optional<int> item, bool expectHasValue, int expectValue)
        {
            bool actualHasValue = item.HasValue(out int actualValue);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, actualValue);
        }

        public static TheoryData<Optional<int>, bool, int> ValueTypeHasValueSamples {
            get {
                TheoryData<Optional<int>, bool, int> data = new()
                {
                    {default, false, default},
                    {new(OptionalState.Null), false, default},
                    {new(OptionalState.Undefined), false, default},
                    {new(0), true, 0},
                    {new(-1), true, -1 },
                    {new(1), true, 1 }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(StringHasValueSamples))]
        public void StringHasValue(Optional<string> item, bool expectHasValue, string? expectValue)
        {
            bool actualHasValue = item.HasValue(out string? actualValue);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, actualValue);
        }

        public static TheoryData<Optional<string>, bool, string?> StringHasValueSamples {
            get {
                TheoryData<Optional<string>, bool, string?> data = new()
                {
                    {default, false, default},
                    {new(OptionalState.Null), false, default},
                    {new(OptionalState.Undefined), false, default},
                    {new(null), false, null},
                    {new(String.Empty), true, String.Empty },
                    {new("Test"), true, "Test" }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(IsSetSamples))]
        public void IsSet(Optional<int> item, bool expectIsSet)
        {
            bool actualIsSet = item.IsSet();

            Assert.Equal(expectIsSet, actualIsSet);
        }

        public static TheoryData<Optional<int>, bool> IsSetSamples {
            get {
                TheoryData<Optional<int>, bool> data = new()
                {
                    {default, false},
                    {new(OptionalState.Null), true},
                    {new(OptionalState.Undefined), false},
                    {new(0), true}
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(ValueTypeIsEqualSamples))]
        public void ValueTypeEqual(Optional<int> item1, Optional<int> item2, bool expectEqual)
        {
            bool actualEqual = item1 == item2;
            bool actualNotEqual = item1 != item2;

            Assert.Equal(expectEqual, actualEqual);
            Assert.Equal(!expectEqual, actualNotEqual);
        }

        public static TheoryData<Optional<int>, Optional<int>, bool> ValueTypeIsEqualSamples {
            get {
                TheoryData<Optional<int>, Optional<int>, bool> data = new()
                {
                    {default, default, true},
                    {new(OptionalState.Null), new(OptionalState.Null), true },
                    {new(OptionalState.Undefined), new(OptionalState.Undefined), true },
                    {new(OptionalState.Null), new(OptionalState.Undefined), false },
                    {new(0), new(0), true },
                    {new(0), new(-1), false },
                    {new(OptionalState.Null), new(0), false },
                    {new(OptionalState.Undefined), new(0), false },
                    {new(0), new(OptionalState.Null), false },
                    {new(0), new(OptionalState.Undefined), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(StringTypeCaseIsEqualSamples))]
        public void StringTypeCaseEqual(Optional<string> item1, Optional<string> item2, bool expectEqual)
        {
            bool actualEqual = item1 == item2;
            bool actualNotEqual = item1 != item2;

            Assert.Equal(expectEqual, actualEqual);
            Assert.Equal(!expectEqual, actualNotEqual);
        }

        public static TheoryData<Optional<string>, Optional<string>, bool> StringTypeCaseIsEqualSamples {
            get {
                TheoryData<Optional<string>, Optional<string>, bool> data = new()
                {
                    {default, default, true},
                    {new(OptionalState.Null), new(OptionalState.Null), true },
                    {new(OptionalState.Undefined), new(OptionalState.Undefined), true },
                    {new(OptionalState.Null), new(OptionalState.Undefined), false },
                    {new("test"), new("test"), true },
                    {new("test"), new("Test"), false },
                    {new("test"), new("abc"), false },
                    {new(null), new("Test"), false },
                    {new(OptionalState.Null), new(String.Empty), false },
                    {new(OptionalState.Undefined), new(String.Empty), false },
                    {new(String.Empty), new(OptionalState.Null), false },
                    {new(String.Empty), new(OptionalState.Undefined), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(StringTypeNotCaseEqualSamples))]
        public void StringTypeNotCaseEqual(Optional<string> item1, Optional<string> item2, bool expectEqual)
        {
            IEqualityComparer<string?> comparer = StringComparer.OrdinalIgnoreCase;
            bool actualEqual = item1.Equals(item2, comparer);

            Assert.Equal(expectEqual, actualEqual);
        }

        public static TheoryData<Optional<string>, Optional<string>, bool> StringTypeNotCaseEqualSamples {
            get {
                TheoryData<Optional<string>, Optional<string>, bool> data = new()
                {
                    {default, default, true},
                    {new(OptionalState.Null), new(OptionalState.Null), true },
                    {new(OptionalState.Undefined), new(OptionalState.Undefined), true },
                    {new(OptionalState.Null), new(OptionalState.Undefined), false },
                    {new("test"), new("test"), true },
                    {new("test"), new("Test"), true },
                    {new("test"), new("abc"), false },
                    {new(null), new("Test"), false },
                    {new(OptionalState.Null), new(String.Empty), false },
                    {new(OptionalState.Undefined), new(String.Empty), false },
                    {new(String.Empty), new(OptionalState.Null), false },
                    {new(String.Empty), new(OptionalState.Undefined), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(HashCodeSamples))]
        public void HashCode(Optional<string> item, int expectHashCode)
        {
            int actualHashCode = item.GetHashCode();

            Assert.Equal(expectHashCode, actualHashCode);
        }

        public static TheoryData<Optional<string>, int> HashCodeSamples {
            get {
                TheoryData<Optional<string>, int> data = new()
                {
                    {default, OptionalState.Undefined.GetHashCode()},
                    {new(OptionalState.Null), OptionalState.Null.GetHashCode() },
                    {new(OptionalState.Undefined), OptionalState.Undefined.GetHashCode() },
                    {new(String.Empty), System.HashCode.Combine(OptionalState.HasValue, String.Empty) },
                    {new("test"), System.HashCode.Combine(OptionalState.HasValue, "test") },
                    {new(null), OptionalState.Null.GetHashCode() }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(ToStringSamples))]
        public void ToStringTest(Optional<string> item, string expectToString)
        {
            string actualToString = item.ToString();
            Assert.Equal(expectToString, actualToString);
        }

        public static TheoryData<Optional<string>, string> ToStringSamples {
            get {
                TheoryData<Optional<string>, string> data = new()
                {
                    {default, "undefined"},
                    {new(OptionalState.Null), "null"},
                    {new(OptionalState.Undefined), "undefined"},
                    {new(String.Empty), String.Empty},
                    {new("test"), "test"},
                    {new(null), "null"},
                    {new("undefined"), "undefined"}
                };
                return data;
            }
        }
    }
}