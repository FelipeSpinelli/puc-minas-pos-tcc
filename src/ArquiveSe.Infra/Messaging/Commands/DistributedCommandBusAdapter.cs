using Amazon.Runtime.Internal.Auth;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Configurations;
using ArquiveSe.Infra.Messaging.Models;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ArquiveSe.Infra.Messaging.Commands;

public class DistributedCommandBusAdapter : CommandBusAdapter, ICommandBusPort
{
    private readonly ServiceBusSender _sender;
    private readonly IServiceProvider? _serviceProvider;
    private readonly ServiceBusProcessor? _processor;

    public const string SENDER_NAME = $"{nameof(Commands)}Sender";
    public const string PROCESSOR_NAME = $"{nameof(Commands)}Processor";

    public DistributedCommandBusAdapter(
        IServiceProvider serviceProvider,
        IAzureClientFactory<ServiceBusSender> serviceBusSenderFactory,
        IAzureClientFactory<ServiceBusProcessor> serviceBusProcessorFactory,
        IMediator bus,
        MessagingSettings settings)
        : this(serviceBusSenderFactory, bus, settings)
    {
        _serviceProvider = serviceProvider;
        _processor = serviceBusProcessorFactory.CreateClient(PROCESSOR_NAME);

        if (_processor.IsProcessing)
        {
            return;
        }

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        _processor.StartProcessingAsync().GetAwaiter().GetResult();
    }

    public DistributedCommandBusAdapter(
        IAzureClientFactory<ServiceBusSender> serviceBusSenderFactory,
        IMediator bus,
        MessagingSettings settings)
        : base(bus, settings)
    {
        _sender = serviceBusSenderFactory.CreateClient(SENDER_NAME);
    }

    public override Task Send<T>(T message)
    {
        var queueCommand = QueueCommand.CreateFrom(message);
        var busMessage = new ServiceBusMessage(JsonSerializer.Serialize(queueCommand));

        return _sender.SendMessageAsync(busMessage);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            using var scope = _serviceProvider!.CreateScope();
            var bus = scope.ServiceProvider.GetRequiredService<IMediator>();
            var body = args.Message.Body.ToString();
            var queueCommand = JsonSerializer.Deserialize<QueueCommand>(body);
            await bus.Send(queueCommand!.GetCommand());

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.ErrorSource);
        Console.WriteLine(args.FullyQualifiedNamespace);
        Console.WriteLine(args.EntityPath);
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}
