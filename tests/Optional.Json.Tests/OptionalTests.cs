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

            [DataMember]
            public Optional<string> String { get; set; }
        }

        public static TheoryData<string, TestModel1> DeserializeModel1Samples {
            get {
                var data = new TheoryData<string, TestModel1>
                {
                    {"{}", new TestModel1()},
                    {
                        "{\"integer\":null, \"string\":null}",
                        new TestModel1
                        {
                            Integer = new Optional<int>(OptionalState.Null),
                            String = new Optional<string>(OptionalState.Null)
                        }
                    },
                    {
                        "{\"integer\":23, \"string\":\"test\"}",
                        new TestModel1
                        {
                            Integer = new Optional<int>(23),
                            String = new Optional<string>("test")
                        }
                    }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(DeserializeModel1Samples))]
        public void DeserializeModel1(string json, TestModel1 expectResult)
        {
            var options = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new OptionalConverter() }
            };

            TestModel1? actualResult = JsonSerializer.Deserialize<TestModel1>(json, options);

            Assert.NotNull(actualResult);

            EnsureEqual(expectResult.Integer, actualResult.Integer);
            EnsureEqual(expectResult.String, actualResult.String);
        }

        private static void EnsureEqual<T>(Optional<T> expect, Optional<T> actual)
        {
            Assert.Equal(expect.State, actual.State);
            Assert.Equal(expect.IsNull(), actual.IsNull());
            Assert.Equal(expect.IsUndefined(), actual.IsUndefined());
            Assert.Equal(expect.IsSet(), actual.IsSet());
            Assert.Equal(expect.HasValue(out T valueE), actual.HasValue(out T valueA));

            Assert.Equal(valueE, valueA);
        }

        public static TheoryData<TestModel1, string> SerializeModel1Samples {
            get {
                var data = new TheoryData<TestModel1, string>
                {
                    {new TestModel1(), "{\"integer\":null,\"string\":null}"},
                    {
                        new TestModel1
                        {
                            Integer = new Optional<int>(OptionalState.Null),
                            String = new Optional<string>(OptionalState.Null)
                        },
                        "{\"integer\":null,\"string\":null}"
                    },
                    {
                        new TestModel1
                        {
                            Integer = new Optional<int>(23),
                            String = new Optional<string>("test")
                        },
                        "{\"integer\":23,\"string\":\"test\"}"
                    }
                };
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(SerializeModel1Samples))]
        public void SerializeModel1(TestModel1 model, string expectJson)
        {
            var options = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new OptionalConverter() }
            };

            string actualJson = JsonSerializer.Serialize(model, options);

            Assert.Equal(expectJson, actualJson);
        }
    }
}