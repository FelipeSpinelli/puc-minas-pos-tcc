using ArquiveSe.Domain.Abstractions.Repositories;
using ArquiveSe.Infra.Data.Repositories;
using ArquiveSe.Infra.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            return services
                .AddSingleton(sp =>
                {
                    var connectionString = sp.GetService<IConfiguration>()?["DatabaseConnectionString"];
                    return new MongoClient(connectionString);
                })
                .AddScoped<IManagedFileRepository, ManagedFileRepository>()
                .AddScoped<IUserRepository, UserRepository>();
        }
        public static IServiceCollection AddBlobStorage(this IServiceCollection services)
        {
            return services
                .AddSingleton(sp =>
                {
                    var connectionString = sp.GetService<IConfiguration>()?["BlobStorageConnectionString"];
                    return new BlobServiceClient(connectionString);
                })
                .AddScoped<IBlobRepository, AzureBlobStorageRepository>();
        }
    }
}
