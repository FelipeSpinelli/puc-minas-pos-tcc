using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Infra.Databases.Read;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Databases.Read.Configurations;

public static class ReadDbPortsExtensions
{
    public static IServiceCollection AddReadingDb(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<IMongoClient>(sp =>
            {
                return new MongoClient(configuration.GetConnectionString("ReadDb"));
            })
            .AddScoped(sp =>
            {
                return sp.GetRequiredService<IMongoClient>().GetDatabase(nameof(ArquiveSe));
            })
            .AddScoped<IDocumentReadDbPort, DocumentReadDbAdapter>()
            .AddScoped<IFolderReadDbPort, FolderReadDbAdapter>()
            .AddScoped<IFlowReadDbPort, FlowReadDbAdapter>();
    }
}
