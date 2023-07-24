using Newtonsoft.Json;
using Optional.NewtonsoftJson;
using Xunit;

namespace Nness.Text.Json.Tests
{
    public partial class OptionalCollectionTests
    {
        [Theory(DisplayName = "Newtonsoft Json Deserialize OptionalCollection")]
        [MemberData(nameof(DeserializeModel1Samples))]
        public void DeserializeModel1ByNewtonsoft(string json, TestModel1 expectResult)
        {
            JsonSerializerSettings settings = new() {
                ContractResolver = new OptionalContractResolver()
            };

            TestModel1? actualResult = JsonConvert.DeserializeObject<TestModel1>(json, settings);

            Assert.NotNull(actualResult);
            if (actualResult == null) {
                return;
            }

            EnsureEqual(expectResult.Integer, actualResult.Integer);
            EnsureEqual(expectResult.String, actualResult.String);
        }

        [Theory(DisplayName = "Newtonsoft Json Serialize OptionalCollection")]
        [MemberData(nameof(SerializeModel1Samples))]
        public void SerializeModel1ByNewtonsoft(TestModel1 model, string expectJson)
        {
            JsonSerializerSettings settings = new() {
                ContractResolver = new OptionalContractResolver()
            };

            string actualJson = JsonConvert.SerializeObject(model, settings);

            Assert.Equal(expectJson, actualJson);
        }
    }
}