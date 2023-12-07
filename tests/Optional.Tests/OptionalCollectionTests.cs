using System;
using System.Collections.Generic;
using Xunit;

namespace DeskDirector.Text.Json.Tests
{
    public class OptionalCollectionTests
    {
        [Theory]
        [MemberData(nameof(HasValueSamples))]
        public void HasValue(OptionalCollection<int> list, bool expectHasValue, ICollection<int>? expectValue)
        {
            bool actualHasValue = list.HasValue(out ICollection<int>? actualValue);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, actualValue);
        }

        public static TheoryData<OptionalCollection<int>, bool, ICollection<int>?> HasValueSamples {
            get {
                TheoryData<OptionalCollection<int>, bool, ICollection<int>?> data = new()
                {
                    {default, false, default},
                    {new(OptionalState.Null), false, default},
                    {new(OptionalState.Undefined), false, default},
                    {new(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new(new[] {1}), true, new[] {1}}
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(IsSetSamples))]
        public void IsSet(OptionalCollection<int> list, bool expectIsSet)
        {
            bool actualIsSet = list.IsSet();

            Assert.Equal(expectIsSet, actualIsSet);
        }

        public static TheoryData<OptionalCollection<int>, bool> IsSetSamples {
            get {
                TheoryData<OptionalCollection<int>, bool> data = new()
                {
                    { default, false },
                    { new(OptionalState.Null), true },
                    { new(OptionalState.Undefined), false},
                    { new(Array.Empty<int>()), true },
                    { new(new[] {1}), true },
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(EnumerableSamples))]
        public void EnumerableTest(OptionalCollection<int> list, bool expectHasValue, IEnumerable<int> expectValue)
        {
            bool actualHasValue = list.HasValue(out ICollection<int>? _);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, list);
        }

        public static TheoryData<OptionalCollection<int>, bool, IEnumerable<int>> EnumerableSamples {
            get {
                TheoryData<OptionalCollection<int>, bool, IEnumerable<int>> data = new()
                {
                    {default, false, Array.Empty<int>()},
                    {new(OptionalState.Null), false, Array.Empty<int>()},
                    {new(OptionalState.Undefined), false, Array.Empty<int>()},
                    {new(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new(new[] {1}), true, new[] {1}}
                };

                return data;
            }
        }

        /// <summary>
        /// Same HashCode only when reference is same. Thus, Array.Empty has same reference, so it
        /// can pass.
        /// </summary>
        [Theory]
        [MemberData(nameof(HashCodeSamples))]
        public void HashCode(OptionalCollection<int> item, int expectHashCode)
        {
            int actualHashCode = item.GetHashCode();

            Assert.Equal(expectHashCode, actualHashCode);
        }

        public static TheoryData<OptionalCollection<int>, int> HashCodeSamples {
            get
            {
                TheoryData<OptionalCollection<int>, int> data = new()
                {
                    { default, OptionalState.Undefined.GetHashCode() },
                    { new(OptionalState.Undefined), OptionalState.Undefined.GetHashCode() },
                    { new(OptionalState.Null), OptionalState.Null.GetHashCode() },
                    {
                        new(Array.Empty<int>()),
                        System.HashCode.Combine(OptionalState.HasValue, Array.Empty<int>().GetHashCode())
                    }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(ToStringSamples))]
        public void ToStringTest(OptionalCollection<int> item, string expectToString)
        {
            string actualToString = item.ToString();
            Assert.Equal(expectToString, actualToString);
        }

        public static TheoryData<OptionalCollection<int>, string> ToStringSamples {
            get {
                TheoryData<OptionalCollection<int>, string> data = new()
                {
                    {default, "undefined"},
                    {new(OptionalState.Undefined), "undefined"},
                    {new(OptionalState.Null), "null"},
                    {new(Array.Empty<int>()), Array.Empty<int>().ToString() ?? String.Empty}
                };

                return data;
            }
        }
    }
}