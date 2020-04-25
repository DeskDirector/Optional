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
                    {new OptionalCollection<int>(OptionalState.Null), false, default},
                    {new OptionalCollection<int>(OptionalState.Undefined), false, default},
                    {new OptionalCollection<int>(Array.Empty<int>()), true, Array.Empty<int>()},
                    {new OptionalCollection<int>(new[] {1}), true, new[] {1}}
                };

                return data;
            }
        }
    }
}