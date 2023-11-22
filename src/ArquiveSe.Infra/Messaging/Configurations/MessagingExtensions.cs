using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Commands;
using ArquiveSe.Infra.Messaging.Events;
using Azure.Messaging.ServiceBus;
using MediatR;
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
            .WithName(DistributedCommandBusAdapter.SENDER_NAME);
            clientsBuilder.AddClient<ServiceBusProcessor, ServiceBusClientOptions>((_, _, provider) =>
                provider
                    .GetService<ServiceBusClient>()!
                    .CreateProcessor(nameof(Commands)))
            .WithName(DistributedCommandBusAdapter.PROCESSOR_NAME);
        });

        return services
            .AddScoped<ICommandBusPort>(sp =>
            {
                _ = sp.GetRequiredService<DistributedCommandBusAdapter>();
                var serviceBusSenderFactory = sp.GetRequiredService<IAzureClientFactory<ServiceBusSender>>();
                var mediator = sp.GetRequiredService<IMediator>();
                var settings = sp.GetRequiredService<MessagingSettings>();

                return new DistributedCommandBusAdapter(serviceBusSenderFactory, mediator, settings);
            })
            .AddSingleton(sp =>
            {
                var serviceBusSenderFactory = sp.GetRequiredService<IAzureClientFactory<ServiceBusSender>>();
                var serviceBusProcessorFactory = sp.GetRequiredService<IAzureClientFactory<ServiceBusProcessor>>();
                var mediator = sp.GetRequiredService<IMediator>();
                var settings = sp.GetRequiredService<MessagingSettings>();

                return new DistributedCommandBusAdapter(sp, serviceBusSenderFactory, serviceBusProcessorFactory, mediator, settings);
            });
    }
}
