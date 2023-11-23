using Amazon.Runtime.Internal.Util;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Configurations;
using ArquiveSe.Infra.Messaging.Models;
using Coravel.Invocable;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArquiveSe.Infra.Messaging.Commands;

public class InMemoryCommandBusAdapter :
    CommandBusAdapter,
    ICommandBusPort,
    IInvocable
{
    private readonly ILogger<InMemoryCommandBusAdapter> _logger;
    private static readonly Queue<QueueCommand> _queue = new();
    private static bool _running;

    public InMemoryCommandBusAdapter(
        IMediator bus,
        MessagingSettings settings,
        ILogger<InMemoryCommandBusAdapter> logger)
        : base(bus, settings)
    {
        _logger = logger;
    }

    public async Task Invoke()
    {
        if (_running)
        {
            return;
        }

        _running = true;
        while (_queue.TryDequeue(out var queueCommand))
        {
            _logger.LogInformation($"Sending {queueCommand.CommandType} command\r\n{queueCommand.CommandData}");
            await _bus.Send(queueCommand.GetCommand());
        }
        _running = false;
    }

    public override Task Send<T>(T message)
    {
        var queueCommand = QueueCommand.CreateFrom(message);
        _queue.Enqueue(queueCommand);
        return Task.CompletedTask;
    }
}
