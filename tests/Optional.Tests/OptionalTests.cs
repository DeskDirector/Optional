using System;
using System.Collections.Generic;
using Xunit;

namespace Nness.Text.Json.Tests
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
                var data = new TheoryData<Optional<int>, bool, int>
                {
                    {new Optional<int>(OptionalState.Null), false, default},
                    {new Optional<int>(OptionalState.Undefined), false, default},
                    {new Optional<int>(0), true, 0},
                    {new Optional<int>(-1), true, -1 },
                    {new Optional<int>(1), true, 1 }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(StringHasValueSamples))]
        public void StringHasValue(Optional<string?> item, bool expectHasValue, string? expectValue)
        {
            bool actualHasValue = item.HasValue(out string? actualValue);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, actualValue);
        }

        public static TheoryData<Optional<string?>, bool, string?> StringHasValueSamples {
            get {
                var data = new TheoryData<Optional<string?>, bool, string?>
                {
                    {new Optional<string?>(OptionalState.Null), false, default},
                    {new Optional<string?>(OptionalState.Undefined), false, default},
                    {new Optional<string?>(null), false, null},
                    {new Optional<string?>(String.Empty), true, String.Empty },
                    {new Optional<string?>("Test"), true, "Test" }
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
                var data = new TheoryData<Optional<int>, bool>
                {
                    { new Optional<int>(OptionalState.Null), true },
                    { new Optional<int>(OptionalState.Undefined), false },
                    { new Optional<int>(0), true }
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
                var data = new TheoryData<Optional<int>, Optional<int>, bool>
                {
                    {new Optional<int>(OptionalState.Null), new Optional<int>(OptionalState.Null), true },
                    {new Optional<int>(OptionalState.Undefined), new Optional<int>(OptionalState.Undefined), true },
                    {new Optional<int>(OptionalState.Null), new Optional<int>(OptionalState.Undefined), false },
                    {new Optional<int>(0), new Optional<int>(0), true },
                    {new Optional<int>(0), new Optional<int>(-1), false },
                    {new Optional<int>(OptionalState.Null), new Optional<int>(0), false },
                    {new Optional<int>(OptionalState.Undefined), new Optional<int>(0), false },
                    {new Optional<int>(0), new Optional<int>(OptionalState.Null), false },
                    {new Optional<int>(0), new Optional<int>(OptionalState.Undefined), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(StringTypeCaseIsEqualSamples))]
        public void StringTypeCaseEqual(Optional<string?> item1, Optional<string?> item2, bool expectEqual)
        {
            bool actualEqual = item1 == item2;
            bool actualNotEqual = item1 != item2;

            Assert.Equal(expectEqual, actualEqual);
            Assert.Equal(!expectEqual, actualNotEqual);
        }

        public static TheoryData<Optional<string?>, Optional<string?>, bool> StringTypeCaseIsEqualSamples {
            get {
                var data = new TheoryData<Optional<string?>, Optional<string?>, bool>
                {
                    {new Optional<string?>(OptionalState.Null), new Optional<string?>(OptionalState.Null), true },
                    {new Optional<string?>(OptionalState.Undefined), new Optional<string?>(OptionalState.Undefined), true },
                    {new Optional<string?>(OptionalState.Null), new Optional<string?>(OptionalState.Undefined), false },
                    {new Optional<string?>("test"), new Optional<string?>("test"), true },
                    {new Optional<string?>("test"), new Optional<string?>("Test"), false },
                    {new Optional<string?>("test"), new Optional<string?>("abc"), false },
                    {new Optional<string?>(null), new Optional<string?>("Test"), false },
                    {new Optional<string?>(OptionalState.Null), new Optional<string?>(String.Empty), false },
                    {new Optional<string?>(OptionalState.Undefined), new Optional<string?>(String.Empty), false },
                    {new Optional<string?>(String.Empty), new Optional<string?>(OptionalState.Null), false },
                    {new Optional<string?>(String.Empty), new Optional<string?>(OptionalState.Undefined), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(StringTypeNotCaseEqualSamples))]
        public void StringTypeNotCaseEqual(Optional<string?> item1, Optional<string?> item2, bool expectEqual)
        {
            IEqualityComparer<string?> comparer = StringComparer.OrdinalIgnoreCase;
            bool actualEqual = item1.Equals(item2, comparer);

            Assert.Equal(expectEqual, actualEqual);
        }

        public static TheoryData<Optional<string?>, Optional<string?>, bool> StringTypeNotCaseEqualSamples {
            get {
                var data = new TheoryData<Optional<string?>, Optional<string?>, bool>
                {
                    {new Optional<string?>(OptionalState.Null), new Optional<string?>(OptionalState.Null), true },
                    {new Optional<string?>(OptionalState.Undefined), new Optional<string?>(OptionalState.Undefined), true },
                    {new Optional<string?>(OptionalState.Null), new Optional<string?>(OptionalState.Undefined), false },
                    {new Optional<string?>("test"), new Optional<string?>("test"), true },
                    {new Optional<string?>("test"), new Optional<string?>("Test"), true },
                    {new Optional<string?>("test"), new Optional<string?>("abc"), false },
                    {new Optional<string?>(null), new Optional<string?>("Test"), false },
                    {new Optional<string?>(OptionalState.Null), new Optional<string?>(String.Empty), false },
                    {new Optional<string?>(OptionalState.Undefined), new Optional<string?>(String.Empty), false },
                    {new Optional<string?>(String.Empty), new Optional<string?>(OptionalState.Null), false },
                    {new Optional<string?>(String.Empty), new Optional<string?>(OptionalState.Undefined), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(HashCodeSamples))]
        public void HashCode(Optional<string?> item, int expectHashCode)
        {
            int actualHashCode = item.GetHashCode();

            Assert.Equal(expectHashCode, actualHashCode);
        }

        public static TheoryData<Optional<string?>, int> HashCodeSamples {
            get {
                var data = new TheoryData<Optional<string?>, int>
                {
                    {new Optional<string?>(OptionalState.Null), 0 },
                    {new Optional<string?>(OptionalState.Undefined), -1 },
                    {new Optional<string?>(String.Empty), String.Empty.GetHashCode() },
                    {new Optional<string?>("test"), "test".GetHashCode() },
                    {new Optional<string?>(null), 0 }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(ToStringSamples))]
        public void ToStringTest(Optional<string?> item, string expectToString)
        {
            string actualToString = item.ToString();
            Assert.Equal(expectToString, actualToString);
        }

        public static TheoryData<Optional<string?>, string> ToStringSamples {
            get {
                var data = new TheoryData<Optional<string?>, string>
                {
                    {new Optional<string?>(OptionalState.Null), "null" },
                    {new Optional<string?>(OptionalState.Undefined), "undefined" },
                    {new Optional<string?>(String.Empty), String.Empty },
                    {new Optional<string?>("test"), "test" },
                    {new Optional<string?>(null), "null" },
                    {new Optional<string?>("undefined"), "undefined" }
                };
                return data;
            }
        }
    }
}