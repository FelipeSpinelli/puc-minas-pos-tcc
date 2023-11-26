using ArquiveSe.Application.Ports.Driven;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiveSe.Infra.Storage.Configurations;
public static class StorageExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthCheckBuilder)
    {
        var settings = configuration.GetSection(StorageSettings.SECTION_NAME).Get<StorageSettings>();
        services.AddSingleton(settings);

        if (settings.UseLocalStorage)
        {
            return services.AddScoped<IFileStoragePort, LocalFileStorageAdapter>();
        }

        healthCheckBuilder.AddAzureBlobStorage(configuration.GetConnectionString(settings.ConnectionStringName));

        return services
                .AddSingleton(sp =>
                {
                    return new BlobServiceClient(configuration.GetConnectionString(settings.ConnectionStringName));
                })
                .AddScoped<IFileStoragePort, AzureBlobFileStorageAdapter>();
    }
}
