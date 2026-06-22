using System.Reflection;
using System.Text.Json;
using DeskDirector.Text.Json;
using DeskDirector.Text.Json.AspNetCore;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Optional.AspNetCore.Tests
{
    public class OptionalSchemaFilterTests
    {
        [Theory]
        [InlineData(nameof(SampleModel.Boolean), false)]
        [InlineData(nameof(SampleModel.Integer), false)]
        [InlineData(nameof(SampleModel.String), false)]
        [InlineData(nameof(SampleModel.Model), false)]
        [InlineData(nameof(SampleModel.ModelArray), false)]
        [InlineData(nameof(SampleModel.NullableBoolean), true)]
        [InlineData(nameof(SampleModel.NullableInteger), true)]
        [InlineData(nameof(SampleModel.NullableString), true)]
        [InlineData(nameof(SampleModel.NullableModel), true)]
        [InlineData(nameof(SampleModel.NullableModelArray), true)]
        public void PropertySchema(string propertyName, bool expectedNullable)
        {
            PropertyInfo? propertyInfo = typeof(SampleModel).GetProperty(propertyName);
            Assert.NotNull(propertyInfo);

            ISchemaGenerator generator = new SchemaGenerator(
                new SchemaGeneratorOptions {
                    SupportNonNullableReferenceTypes = true,
                    UseAllOfToExtendReferenceSchemas = true
                },
                new JsonSerializerDataContractResolver(new JsonSerializerOptions())
            );

            SchemaFilterContext context = new(
                type: propertyInfo.PropertyType,
                schemaGenerator: generator,
                schemaRepository: new SchemaRepository(),
                memberInfo: propertyInfo
            );

            IOpenApiSchema schema = context.SchemaGenerator.GenerateSchema(
                 propertyInfo.PropertyType,
                 context.SchemaRepository,
                 context.MemberInfo,
                 context.ParameterInfo
             );

            new OptionalSchemaFilter().Apply(schema, context);

            Assert.Equal(expectedNullable, IsNullable(schema));

            if (schema.OneOf == null) {
                return;
            }

            Assert.Null(schema.AllOf);
            Assert.Null(schema.AnyOf);
        }

        public static bool IsNullable(IOpenApiSchema schema)
        {
            if (schema.Type?.HasFlag(JsonSchemaType.Null) == true) {
                return true;
            }

            return schema.OneOf?.Any(s => s.Type == JsonSchemaType.Null) == true;
        }

        public class SampleModel
        {
            public Optional<bool> Boolean { get; set; }

            public Optional<int> Integer { get; set; }

            public Optional<string> String { get; set; }

            public Optional<PropertyModel> Model { get; set; }

            public OptionalCollection<PropertyModel> ModelArray { get; set; }

            [Nullable]
            public Optional<bool> NullableBoolean { get; set; }

            [Nullable]
            public Optional<int> NullableInteger { get; set; }

            [Nullable]
            public Optional<string> NullableString { get; set; }

            [Nullable]
            public Optional<PropertyModel> NullableModel { get; set; }

            [Nullable]
            public OptionalCollection<PropertyModel> NullableModelArray { get; set; }
        }

        public class PropertyModel
        {
            public string? Property1 { get; set; }
        }
    }
}