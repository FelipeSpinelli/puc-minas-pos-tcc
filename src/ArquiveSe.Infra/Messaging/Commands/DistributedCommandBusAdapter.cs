using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Configurations;
using ArquiveSe.Infra.Messaging.Models;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Azure;
using System.Text.Json;

namespace ArquiveSe.Infra.Messaging.Commands;

public class DistributedCommandBusAdapter : CommandBusAdapter, ICommandBusPort
{
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusReceiver _receiver;
    private readonly System.Timers.Timer _timer;
    private readonly Timer _timer2;


    public DistributedCommandBusAdapter(
        IAzureClientFactory<ServiceBusSender> serviceBusSenderFactory,
        IAzureClientFactory<ServiceBusReceiver> serviceBusReceiverFactory,
        IMediator bus,
        MessagingSettings settings)
        : base(bus, settings)
    {
        _sender = serviceBusSenderFactory.CreateClient(nameof(Commands));
        _receiver = serviceBusReceiverFactory.CreateClient(nameof(Commands));
        _timer = new System.Timers.Timer(200)
        {
            AutoReset = true,
            Enabled = true
        };
        _timer.Elapsed += OnTimerElapsed;

        _tim
    }

    public override Task Send<T>(T message)
    {
        var queueCommand = QueueCommand.CreateFrom(message);
        var busMessage = new ServiceBusMessage(JsonSerializer.Serialize(queueCommand));

        return _sender.SendMessageAsync(busMessage);
    }

    private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
