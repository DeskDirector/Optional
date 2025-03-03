using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using Xunit;

namespace DeskDirector.Text.Json.Tests
{
    public partial class OptionalCollectionTests
    {
        [DataContract]
        public class TestModel1
        {
            [DataMember]
            public OptionalCollection<int> Integer { get; set; }

            [DataMember]
            public OptionalCollection<string> String { get; set; }

            [DataMember]
            public OptionalCollection<TestObject> Object { get; set; }
        }

        public class TestObject
        {
            public string? Value { get; set; }
        }

        public static TheoryData<string, TestModel1> DeserializeModel1Samples {
            get {
                TheoryData<string, TestModel1> data = new() {
                    {"{}", new TestModel1()},
                    {
                        """
                        {"integer":null, "string":null, "object":null}
                        """,
                        new TestModel1
                        {
                            Integer = OptionalCollection<int>.Null,
                            String = OptionalCollection<string>.Null,
                            Object = OptionalCollection<TestObject>.Null
                        }
                    },
                    {
                        """
                        {"integer":[23], "string":["test"], "object":[{"value":"this is object"}]}
                        """,
                        new TestModel1
                        {
                            Integer = OptionalCollection<int>.WithValue([23]),
                            String = OptionalCollection<string>.WithValue(["test"]),
                            Object = OptionalCollection<TestObject>.WithValue([new TestObject { Value = "this is object" }])
                        }
                    }
                };
                return data;
            }
        }

        [Theory(DisplayName = "System.Text.Json Deserialize OptionalCollection")]
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

        private static void EnsureEqual<T>(OptionalCollection<T> expect, OptionalCollection<T> actual)
        {
            Assert.Equal(expect.State, actual.State);
            Assert.Equal(expect.IsNull(), actual.IsNull());
            Assert.Equal(expect.IsUndefined(), actual.IsUndefined());
            Assert.Equal(expect.IsSet(), actual.IsSet());
            Assert.Equal(
                expect.HasValue(out IReadOnlyCollection<T>? valueE),
                actual.HasValue(out IReadOnlyCollection<T>? valueA)
            );

            if (valueE == null) {
                Assert.Null(valueA);
                return;
            }

            Assert.NotNull(valueA);
            Assert.Equal(valueE, valueA);
        }

        public static TheoryData<TestModel1, string> SerializeModel1Samples {
            get {
                TheoryData<TestModel1, string> data = new() {
                    { new TestModel1(), "{}" },
                    {
                        new TestModel1
                        {
                            Integer = OptionalCollection<int>.Undefined,
                            String = OptionalCollection<string>.Undefined,
                            Object = OptionalCollection<TestObject>.Undefined
                        },
                        "{}"
                    },
                    {
                        new TestModel1
                        {
                            Integer = OptionalCollection<int>.Null,
                            String = OptionalCollection<string>.Null,
                            Object = OptionalCollection<TestObject>.Null
                        },
                        """
                        {"integer":null,"string":null,"object":null}
                        """
                    },
                    {
                        new TestModel1
                        {
                            Integer = OptionalCollection<int>.WithValue([23]),
                            String = OptionalCollection<string>.WithValue(["test"]),
                            Object = OptionalCollection<TestObject>.WithValue([new TestObject { Value = "this is object" }])
                        },
                        """
                        {"integer":[23],"string":["test"],"object":[{"value":"this is object"}]}
                        """
                    }
                };
                return data;
            }
        }

        [Theory(DisplayName = "System.Text.Json Serialize OptionalCollection")]
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

        public static TheoryData<Type, bool> OptionalCollectionTypeSamples {
            get {
                TheoryData<Type, bool> data = new() {
                    { typeof(OptionalCollection<int>), true },
                    { typeof(OptionalCollection<string>), true },
                    { typeof(OptionalCollection<DateTime>), true },
                    { typeof(OptionalCollection<object>), true },
                    { typeof(OptionalCollection<TestModel1>), true },
                    { typeof(Optional<int>), false },
                    { typeof(Optional<string>), false },
                    { typeof(Optional<DateTime>), false },
                    { typeof(Optional<object>), false },
                    { typeof(Optional<TestModel1>), false },
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
        [MemberData(nameof(OptionalCollectionTypeSamples))]
        public void IsSettableCollection(Type type, bool expected)
        {
            bool actual = OptionalCollectionConverter.IsOptional(type);
            Assert.Equal(expected, actual);
        }
    }
}