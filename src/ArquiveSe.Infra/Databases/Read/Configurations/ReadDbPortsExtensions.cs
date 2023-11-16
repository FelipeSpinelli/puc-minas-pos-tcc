using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Infra.Databases.Read;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection;

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
