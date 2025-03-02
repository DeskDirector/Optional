using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeskDirector.Text.Json.AspNetCore
{
    public class OptionalSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (IsOptionalCollection(context.Type, out Type? child)) {
                ApplyInnerType(schema, context, child.MakeArrayType());
                return;
            }

            if (!IsOptional(context.Type, out child)) {
                return;
            }

            ApplyInnerType(schema, context, child);
        }

        private static void ApplyInnerType(
            OpenApiSchema schema,
            SchemaFilterContext context,
            Type type)
        {
            OpenApiSchema from = context.SchemaGenerator.GenerateSchema(
                type,
                context.SchemaRepository,
                context.MemberInfo,
                context.ParameterInfo
            );

            schema.PopulateWith(from);

            MemberInfo? memberInfo = context.MemberInfo;
            NullableAttribute? attribute = memberInfo?.GetCustomAttribute<NullableAttribute>();

            if (attribute != null) {
                schema.Nullable = true;
            }
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
    }
}