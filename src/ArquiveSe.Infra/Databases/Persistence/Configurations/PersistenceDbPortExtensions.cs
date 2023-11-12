using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Infra.Databases.Persistence;
using ArquiveSe.Infra.Databases.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenceDbPortExtensions
{
    public static IServiceCollection AddPersistenceDb(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<PersistenceDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("PersistenceDb")))
            .AddScoped<IPersistenceDbPort, PersistenceDbAdapter>();
    }
}
