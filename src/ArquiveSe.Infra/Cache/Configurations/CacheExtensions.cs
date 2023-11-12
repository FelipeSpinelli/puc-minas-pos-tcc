using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Infra.Cache;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDistributedMemoryCache()
            .AddScoped<IOperationLockerPort, OperationLockerAdapter>();
    }
}
