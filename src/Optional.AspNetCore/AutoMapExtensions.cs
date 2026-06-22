using System.Reflection;
using Microsoft.OpenApi;

namespace DeskDirector.Text.Json.AspNetCore
{
    internal static class AutoMapExtensions
    {
        internal static void PopulateWith(this OpenApiSchema to, IOpenApiSchema from)
        {
            ArgumentNullException.ThrowIfNull(to);
            ArgumentNullException.ThrowIfNull(from);

            Type type = typeof(OpenApiSchema);

            foreach (PropertyInfo property in type.GetPublicProperties()
                         .Where(p => p is { CanRead: true, CanWrite: true } && p.CanPublicRead() && p.CanPublicSet())) {
                object? propertyValue = property.GetValue(from);
                property.SetValue(to, propertyValue);
            }
        }

        internal static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        internal static bool CanPublicRead(this PropertyInfo property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return property.CanRead && property.GetGetMethod(false) != null;
        }

        internal static bool CanPublicSet(this PropertyInfo property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return property.CanWrite && property.GetSetMethod(false) != null;
        }
    }
}