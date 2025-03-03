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
            bool actualHasValue = list.HasValue(out IReadOnlyCollection<int>? actualValue);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, actualValue);
        }

        public static TheoryData<OptionalCollection<int>, bool, ICollection<int>?> HasValueSamples {
            get {
                TheoryData<OptionalCollection<int>, bool, ICollection<int>?> data = new()
                {
                    {default, false, null},
                    {new OptionalCollection<int>(OptionalState.Null), false, null},
                    {new OptionalCollection<int>(OptionalState.Undefined), false, null},
                    {new OptionalCollection<int>(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new OptionalCollection<int>([1]), true, [1] }
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
                    { new OptionalCollection<int>(OptionalState.Null), true },
                    { new OptionalCollection<int>(OptionalState.Undefined), false},
                    { new OptionalCollection<int>(Array.Empty<int>()), true },
                    { new OptionalCollection<int>([1]), true },
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(EnumerableSamples))]
        public void EnumerableTest(OptionalCollection<int> list, bool expectHasValue, IEnumerable<int> expectValue)
        {
            bool actualHasValue = list.HasValue(out IReadOnlyCollection<int>? _);

            Assert.Equal(expectHasValue, actualHasValue);
            Assert.Equal(expectValue, list);
        }

        public static TheoryData<OptionalCollection<int>, bool, IEnumerable<int>> EnumerableSamples {
            get {
                TheoryData<OptionalCollection<int>, bool, IEnumerable<int>> data = new()
                {
                    {default, false, Array.Empty<int>()},
                    {new OptionalCollection<int>(OptionalState.Null), false, Array.Empty<int>()},
                    {new OptionalCollection<int>(OptionalState.Undefined), false, Array.Empty<int>()},
                    {new OptionalCollection<int>(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new OptionalCollection<int>([1]), true, [1] }
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
                    { new OptionalCollection<int>(OptionalState.Undefined), OptionalState.Undefined.GetHashCode() },
                    { new OptionalCollection<int>(OptionalState.Null), OptionalState.Null.GetHashCode() },
                    {
                        new OptionalCollection<int>(Array.Empty<int>()),
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
                    {new OptionalCollection<int>(OptionalState.Undefined), "undefined"},
                    {new OptionalCollection<int>(OptionalState.Null), "null"},
                    {new OptionalCollection<int>(Array.Empty<int>()), Array.Empty<int>().ToString() ?? String.Empty}
                };

                return data;
            }
        }
    }
}