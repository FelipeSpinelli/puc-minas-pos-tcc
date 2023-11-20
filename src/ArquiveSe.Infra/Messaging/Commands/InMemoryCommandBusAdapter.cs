using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Configurations;
using ArquiveSe.Infra.Messaging.Models;
using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using MediatR;

namespace ArquiveSe.Infra.Messaging.Commands;

public class InMemoryCommandBusAdapter :
    CommandBusAdapter,
    ICommandBusPort,
    IInvocable,
    IInvocableWithPayload<QueueCommand>
{
    private readonly IQueue _queue;
    public InMemoryCommandBusAdapter(
        IQueue queue,
        IMediator bus,
        MessagingSettings settings)
        : base(bus, settings)
    {
        _queue = queue;
    }

    public QueueCommand Payload { get; set; } = new();

    public Task Invoke() => _bus.Send(Payload);

    public override Task Send<T>(T message)
    {
        var queueCommand = QueueCommand.CreateFrom(message);
        _queue.QueueInvocableWithPayload<InMemoryCommandBusAdapter, QueueCommand>(queueCommand);
        return Task.CompletedTask;
    }
}
