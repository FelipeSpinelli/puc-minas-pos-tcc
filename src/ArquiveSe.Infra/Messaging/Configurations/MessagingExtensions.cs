using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(MessagingExtensions).Assembly);
            })
            .AddScoped<ICommandBusPort, CommandBusAdapter>()
            .AddScoped<IEventPublisherPort, EventPublisherAdapter>();
    }
}
