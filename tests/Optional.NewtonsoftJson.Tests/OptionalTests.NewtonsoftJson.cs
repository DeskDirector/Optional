﻿using System;
using Newtonsoft.Json;
using DeskDirector.Text.Json.NewtonsoftJson;
using Xunit;

namespace DeskDirector.Text.Json.Tests
{
    public partial class OptionalTests
    {
        [Theory(DisplayName = "Newtonsoft Json Deserialize Optional")]
        [MemberData(nameof(DeserializeModel1Samples))]
        public void DeserializeModel1ByNewtonsoft(string json, TestModel1 expectResult)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = new OptionalContractResolver()
            }.AppendIOptionalConverters();

            TestModel1? actualResult = JsonConvert.DeserializeObject<TestModel1>(json, settings);

            Assert.NotNull(actualResult);

            EnsureEqual(expectResult.Integer, actualResult.Integer);
            EnsureEqual(expectResult.String, actualResult.String);
        }

        [Theory(DisplayName = "Newtonsoft Json Serialize Optional")]
        [MemberData(nameof(SerializeModel1Samples))]
        public void SerializeModel1ByNewtonsoft(TestModel1 model, string expectJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = new OptionalContractResolver(),
            }.AppendIOptionalConverters();

            string actualJson = JsonConvert.SerializeObject(model, settings);

            Assert.Equal(expectJson, actualJson);
        }

        [Theory]
        [MemberData(nameof(OptionalTypeSamples))]
        public void IsOptionalByNewtonSoft(Type type, bool expected)
        {
            bool actual = OptionalTypedConverter.IsOptionalType(type);
            Assert.Equal(expected, actual);
        }
    }
}