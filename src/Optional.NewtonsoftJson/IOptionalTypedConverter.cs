using Newtonsoft.Json.Linq;
using Nness.Text.Json;

namespace Optional.NewtonsoftJson
{
    public interface IOptionalTypedConverter
    {
        IOptional Null { get; }

        IOptional Undefined { get; }

        IOptional Value(JToken? token);
    }
}