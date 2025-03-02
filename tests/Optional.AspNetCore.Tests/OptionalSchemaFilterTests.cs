using System.Reflection;
using System.Text.Json;
using DeskDirector.Text.Json;
using DeskDirector.Text.Json.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Optional.AspNetCore.Tests
{
    public class OptionalSchemaFilterTests
    {
        [Theory]
        [InlineData(nameof(SampleModel.Model), false)]
        [InlineData(nameof(SampleModel.ModelArray), false)]
        [InlineData(nameof(SampleModel.NullableModel), true)]
        [InlineData(nameof(SampleModel.NullableModelArray), true)]
        public void PropertySchema(string propertyName, bool expectedNullable)
        {
            OpenApiSchema schema = new();
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

            new OptionalSchemaFilter().Apply(schema, context);

            Assert.Equal(expectedNullable, schema.Nullable);
        }

        public class SampleModel
        {
            public Optional<PropertyModel> Model { get; set; }

            public OptionalCollection<PropertyModel> ModelArray { get; set; }

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