using System;
using System.Runtime.Serialization;
using System.Text.Json;
using Xunit;

namespace DeskDirector.Text.Json.Tests
{
    public partial class OptionalTests
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
                TheoryData<string, TestModel1> data = new() {
                    {"{}", new TestModel1()},
                    {
                        """
                        {"integer":null, "string":null}
                        """,
                        new TestModel1
                        {
                            Integer = new Optional<int>(OptionalState.Null),
                            String = new Optional<string>(OptionalState.Null)
                        }
                    },
                    {
                        """
                        {"integer":23, "string":"test"}
                        """,
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

        [Theory(DisplayName = "System.Text.Json Deserialize Optional")]
        [MemberData(nameof(DeserializeModel1Samples))]
        public void DeserializeModel1(string json, TestModel1 expectResult)
        {
            JsonSerializerOptions options = new() {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                TypeInfoResolver = OptionalJsonTypeInfoResolver.Default
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
            Assert.Equal(expect.HasValue(out T? valueE), actual.HasValue(out T? valueA));

            Assert.Equal(valueE, valueA);
        }

        public static TheoryData<TestModel1, string> SerializeModel1Samples {
            get {
                TheoryData<TestModel1, string> data = new() {
                    { new TestModel1(), "{}" },
                    {
                        new TestModel1
                        {
                            Integer = new Optional<int>(OptionalState.Undefined),
                            String = new Optional<string>(OptionalState.Undefined)
                        },
                        "{}"
                    },
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

        [Theory(DisplayName = "System.Text.Json Serialize Optional")]
        [MemberData(nameof(SerializeModel1Samples))]
        public void SerializeModel1(TestModel1 model, string expectJson)
        {
            JsonSerializerOptions options = new() {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                TypeInfoResolver = OptionalJsonTypeInfoResolver.Default
            };

            string actualJson = JsonSerializer.Serialize(model, options);

            Assert.Equal(expectJson, actualJson);
        }

        public static TheoryData<Type, bool> OptionalTypeSamples {
            get {
                TheoryData<Type, bool> data = new() {
                    { typeof(OptionalCollection<int>), false },
                    { typeof(OptionalCollection<string>), false },
                    { typeof(OptionalCollection<DateTime>), false },
                    { typeof(OptionalCollection<object>), false },
                    { typeof(OptionalCollection<TestModel1>), false },
                    { typeof(Optional<int>), true },
                    { typeof(Optional<string>), true },
                    { typeof(Optional<DateTime>), true },
                    { typeof(Optional<object>), true },
                    { typeof(Optional<TestModel1>), true },
                    { typeof(TestModel1), false },
                    { typeof(int), false },
                    { typeof(int?), false },
                    { typeof(string), false },
                    { typeof(DateTime), false }
                };

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(OptionalTypeSamples))]
        public void IsOptional(Type type, bool expected)
        {
            bool actual = OptionalConverter.IsOptional(type);
            Assert.Equal(expected, actual);
        }
    }
}