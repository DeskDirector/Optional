using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeskDirector.Text.Json.AspNetCore
{
    public static class OptionalServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenOptionalTypeSupport(this IServiceCollection services)
        {
            for (int index = services.Count - 1; index >= 0; index--) {
                ServiceDescriptor current = services[index];
                if (current.ServiceType != typeof(ISerializerDataContractResolver)) {
                    continue;
                }

                services[index] = Replace(current);
                return services;
            }

            throw new InvalidOperationException("Please register Swashbuckle first.");
        }

        private static ServiceDescriptor Replace(ServiceDescriptor existing)
        {
            return existing switch {
                { ImplementationFactory: { } factory } => new ServiceDescriptor(
                    typeof(ISerializerDataContractResolver),
                    sp => new OptionalSerializerDataContractResolver((ISerializerDataContractResolver)factory(sp)),
                    existing.Lifetime
                ),

                { ImplementationInstance: { } instance } => new ServiceDescriptor(
                    typeof(ISerializerDataContractResolver),
                    new OptionalSerializerDataContractResolver((ISerializerDataContractResolver)instance),
                    ServiceLifetime.Singleton // Instance registrations are inherently Singleton
                ),

                { ImplementationType: { } implType } => new ServiceDescriptor(
                    typeof(ISerializerDataContractResolver),
                    provider => new OptionalSerializerDataContractResolver(
                        (ISerializerDataContractResolver)provider.GetRequiredService(implType)
                    ),
                    existing.Lifetime
                ),

                _ => throw new InvalidOperationException(
                    $"Unexpected ServiceDescriptor shape for {nameof(ISerializerDataContractResolver)}"
                )
            };
        }
    }
}