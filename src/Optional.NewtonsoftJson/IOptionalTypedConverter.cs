using Newtonsoft.Json.Linq;

namespace Nness.Text.Json.NewtonsoftJson
{
    public interface IOptionalTypedConverter
    {
        IOptional Null { get; }

        IOptional Undefined { get; }

        IOptional Value(JToken? token);
    }
}