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

            if (IsOptionalCollection(context.Type, out Type? child)) {
                ApplyInnerType(concrete, context, child.MakeArrayType());
                return;
            }

            if (!IsOptional(context.Type, out child)) {
                return;
            }

            ApplyInnerType(concrete, context, child);
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

            schema.PopulateWith(from);

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

        private static bool IsOptionalCollection(
            Type type,
            [NotNullWhen(true)] out Type? value)
        {
            value = null;

            if (!type.IsGenericType ||
                type.GetGenericTypeDefinition() != typeof(OptionalCollection<>)) {
                return false;
            }

            Type? itemType = type.GetGenericArguments().FirstOrDefault();
            if (itemType == null) {
                throw new InvalidOperationException("OptionalCollection<T> doesn't have GenericArguments");
            }

            if (typeof(IOptional).IsAssignableFrom(itemType)) {
                throw new InvalidOperationException("OptionalCollection<T>'s child type T is another optional type.");
            }

            value = itemType;
            return true;
        }

        private static bool IsOptional(
            Type type,
            [NotNullWhen(true)] out Type? value)
        {
            value = null;

            if (!type.IsGenericType ||
                type.GetGenericTypeDefinition() != typeof(Optional<>)) {
                return false;
            }

            Type? itemType = type.GetGenericArguments().FirstOrDefault();
            if (itemType == null) {
                throw new InvalidOperationException("Optional<T> doesn't have GenericArguments");
            }

            if (typeof(IOptional).IsAssignableFrom(itemType)) {
                throw new InvalidOperationException("Optional<T>'s child type T is another optional type.");
            }

            value = itemType;
            return true;
        }

        protected virtual void ModifySchema(OpenApiSchema schema, SchemaFilterContext context)
        { }
    }
}