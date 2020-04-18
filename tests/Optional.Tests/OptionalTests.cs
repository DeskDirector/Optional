using System;
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
    }
}