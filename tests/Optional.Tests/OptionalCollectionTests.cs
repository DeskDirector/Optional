using System;
using System.Collections.Generic;
using Xunit;

namespace Nness.Text.Json.Tests
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
                var data = new TheoryData<OptionalCollection<int>, bool, ICollection<int>?>
                {
                    {default, false, default},
                    {new OptionalCollection<int>(OptionalState.Null), false, default},
                    {new OptionalCollection<int>(OptionalState.Undefined), false, default},
                    {new OptionalCollection<int>(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new OptionalCollection<int>(new[] {1}), true, new[] {1}}
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
                var data = new TheoryData<OptionalCollection<int>, bool>
                {
                    { default, false },
                    { new OptionalCollection<int>(OptionalState.Null), true },
                    { new OptionalCollection<int>(OptionalState.Undefined), false},
                    { new OptionalCollection<int>(Array.Empty<int>()), true },
                    { new OptionalCollection<int>(new[] {1}), true },
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
                var data = new TheoryData<OptionalCollection<int>, bool, IEnumerable<int>>
                {
                    {default, false, Array.Empty<int>()},
                    {new OptionalCollection<int>(OptionalState.Null), false, Array.Empty<int>()},
                    {new OptionalCollection<int>(OptionalState.Undefined), false, Array.Empty<int>()},
                    {new OptionalCollection<int>(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new OptionalCollection<int>(new[] {1}), true, new[] {1}}
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
            get {
                var data = new TheoryData<OptionalCollection<int>, int>
                {
                    {default, -1},
                    {new OptionalCollection<int>(OptionalState.Undefined), -1},
                    {new OptionalCollection<int>(OptionalState.Null), 0},
                    {new OptionalCollection<int>(Array.Empty<int>()), Array.Empty<int>().GetHashCode()}
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
                var data = new TheoryData<OptionalCollection<int>, string>
                {
                    {default, "undefined"},
                    {new OptionalCollection<int>(OptionalState.Undefined), "undefined"},
                    {new OptionalCollection<int>(OptionalState.Null), "null"},
                    {new OptionalCollection<int>(Array.Empty<int>()), Array.Empty<int>().ToString() ?? String.Empty}
                };

                return data;
            }
        }
    }
}