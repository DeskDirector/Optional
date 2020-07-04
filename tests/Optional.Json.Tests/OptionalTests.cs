using System.Runtime.Serialization;
using System.Text.Json;
using Xunit;

namespace Nness.Text.Json.Tests
{
    public class OptionalTests
    {
        [DataContract]
        public class TestModel1
        {
            [DataMember]
            public Optional<int> Integer { get; set; }
        }

        public static TheoryData<string, TestModel1> DeserializeModel1Samples {
            get {
                var data = new TheoryData<string, TestModel1>
                {
                    {"{}", new TestModel1()},
                    {"{\"integer\":null}", new TestModel1 {Integer = new Optional<int>(OptionalState.Null)}},
                    {"{\"integer\":23}", new TestModel1 {Integer = new Optional<int>(23)}}
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(DeserializeModel1Samples))]
        public void DeserializeModel1(string json, TestModel1 expectResult)
        {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                Converters = { new OptionalConverter() }
            };

            TestModel1? actualResult = JsonSerializer.Deserialize<TestModel1>(json, options);

            Assert.NotNull(actualResult);

            Assert.Equal(expectResult.Integer.State, actualResult.Integer.State);
            Assert.Equal(expectResult.Integer.HasValue(out int expect), actualResult.Integer.HasValue(out int actual));
            Assert.Equal(expect, actual);
        }
    }
}