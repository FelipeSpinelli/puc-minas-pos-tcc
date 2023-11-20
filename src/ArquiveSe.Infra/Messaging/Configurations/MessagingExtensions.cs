using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Commands;
using ArquiveSe.Infra.Messaging.Events;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiveSe.Infra.Messaging.Configurations;
public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(MessagingSettings.SECTION_NAME).Get<MessagingSettings>();
        services
            .AddSingleton(settings)
            .AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(MessagingExtensions).Assembly);
            })
            .AddScoped<IEventPublisherPort, EventPublisherAdapter>();

        if (settings.UseInMemory)
        {
            return services
                .AddScoped<ICommandBusPort, InMemoryCommandBusAdapter>()
                .AddTransient<InMemoryCommandBusAdapter>();
        }

        services.AddAzureClients(clientsBuilder =>
        {
            clientsBuilder.AddServiceBusClient(configuration.GetConnectionString(settings.ConnectionStringName))
              .ConfigureOptions(options =>
              {
                  options.RetryOptions.Delay = TimeSpan.FromMilliseconds(50);
                  options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(5);
                  options.RetryOptions.MaxRetries = 3;
              });
            clientsBuilder.AddClient<ServiceBusSender, ServiceBusClientOptions>((_, _, provider) =>
                provider
                    .GetService<ServiceBusClient>()!
                    .CreateSender(nameof(Commands))
            )
            .WithName($"{nameof(ICommandBusPort)}Sender");
            clientsBuilder.AddClient<ServiceBusReceiver, ServiceBusClientOptions>((_, _, provider) =>
            {
                var receiver = provider
                    .GetService<ServiceBusClient>()!
                    .CreateReceiver(nameof(Commands));

                return receiver;
            })
            .WithName($"{nameof(ICommandBusPort)}Receiver");
        });

        return services.AddSingleton<ICommandBusPort, DistributedCommandBusAdapter>();
    }
}
