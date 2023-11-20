using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Infra.Databases.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiveSe.Infra.Databases.Persistence.Configurations;

public static class PersistenceDbPortExtensions
{
    public static IServiceCollection AddPersistenceDb(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<PersistenceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PersistenceDb"));
            })
            .AddScoped<IPersistenceDbPort>(sp =>
            {
                var db = sp.GetRequiredService<PersistenceDbContext>();
                var eventPublisher = sp.GetRequiredService<IEventPublisherPort>();
                db.Database.EnsureCreated();

                return new PersistenceDbAdapter(db, eventPublisher);
            });
    }
}
