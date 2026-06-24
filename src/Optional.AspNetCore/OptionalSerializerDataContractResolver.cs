using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeskDirector.Text.Json.AspNetCore
{
    public class OptionalSerializerDataContractResolver : ISerializerDataContractResolver
    {
        private readonly ISerializerDataContractResolver _inner;

        public OptionalSerializerDataContractResolver(ISerializerDataContractResolver inner)
        {
            ArgumentNullException.ThrowIfNull(inner);

            _inner = inner;
        }

        public DataContract GetDataContractForType(Type type)
        {
            Type effectiveType = OptionalReflection.IsOptional(type, out Type? value) ? value : type;

            DataContract contract = _inner.GetDataContractForType(effectiveType);
            if (contract.DataType != DataType.Object) {
                return contract;
            }

            DataProperty[] properties = contract.ObjectProperties.Select(CheckPropertyNullable).ToArray();
            return DataContract.ForObject(
                contract.UnderlyingType,
                properties,
                contract.ObjectExtensionDataType,
                contract.ObjectTypeNameProperty,
                contract.ObjectTypeNameValue,
                contract.JsonConverter
            );
        }

        private static DataProperty CheckPropertyNullable(DataProperty property)
        {
            if (!OptionalReflection.IsOptional(property.MemberType, out Type? effectiveType)) {
                return property;
            }

            bool isNullable = Nullable.GetUnderlyingType(effectiveType) != null ||
                              property.MemberInfo.GetCustomAttribute<NullableAttribute>(true) != null;
            return new DataProperty(
                property.Name,
                property.MemberType,
                property.IsRequired,
                isNullable,
                property.IsReadOnly,
                property.IsWriteOnly,
                property.MemberInfo
            );
        }
    }
}