using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Infra.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class StorageExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .AddSingleton(sp =>
                {
                    return new BlobServiceClient(configuration.GetConnectionString("FileStorage"));
                })
                .AddScoped<IFileStoragePort, AzureBlobFileStorageAdapter>();
    }
}
