using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeskDirector.Text.Json.AspNetCore
{
    public class OptionalSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema is not OpenApiSchema concrete) {
                return;
            }

            if (!OptionalReflection.IsOptional(context.Type, out Type? effectiveType)) {
                return;
            }

            ApplyInnerType(concrete, context, effectiveType);
        }

        private void ApplyInnerType(
            OpenApiSchema schema,
            SchemaFilterContext context,
            Type type)
        {
            IOpenApiSchema from = context.SchemaGenerator.GenerateSchema(
                type,
                context.SchemaRepository,
                context.MemberInfo,
                context.ParameterInfo
            );

            switch (from) {
                case OpenApiSchema concrete:
                    schema.PopulateWith(concrete);
                    break;

                case OpenApiSchemaReference schemaRef:
                    OpenApiSchema reference = new() {
                        OneOf = [schemaRef]
                    };
                    schema.PopulateWith(reference);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Type IOpenApiSchema({from.GetType().FullName}) is not supported."
                    );
            }

            ModifySchema(schema, context);

            MemberInfo? memberInfo = context.MemberInfo;
            NullableAttribute? attribute = memberInfo?.GetCustomAttribute<NullableAttribute>();

            if (attribute == null) {
                return;
            }

            if (schema.Type != null) {
                if (!schema.Type.Value.HasFlag(JsonSchemaType.Null)) {
                    schema.Type |= JsonSchemaType.Null;
                }
                return;
            }

            if (IsAllOfWithoutNull(schema, out IList<IOpenApiSchema>? allOf)) {
                schema.OneOf = [.. allOf, new OpenApiSchema { Type = JsonSchemaType.Null }];
                schema.AllOf = null;
            }

            if (IsOneOfWithoutNull(schema, out IList<IOpenApiSchema>? oneOf)) {
                schema.OneOf = [.. oneOf, new OpenApiSchema { Type = JsonSchemaType.Null }];
            }
        }

        private static bool IsOneOfWithoutNull(
            IOpenApiSchema schema,
            [NotNullWhen(true)] out IList<IOpenApiSchema>? oneOf)
        {
            oneOf = schema.OneOf;

            return oneOf != null && oneOf.All(s => s.Type != JsonSchemaType.Null);
        }

        private static bool IsAllOfWithoutNull(
            IOpenApiSchema schema,
            [NotNullWhen(true)] out IList<IOpenApiSchema>? allOf)
        {
            allOf = schema.AllOf;

            return allOf is [{ Type: null }];
        }

        protected virtual void ModifySchema(OpenApiSchema schema, SchemaFilterContext context)
        { }
    }
}