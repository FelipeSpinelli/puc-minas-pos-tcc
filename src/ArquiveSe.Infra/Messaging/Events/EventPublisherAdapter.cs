using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Shared;
using MediatR;

namespace ArquiveSe.Infra.Messaging.Events;

public class EventPublisherAdapter : IEventPublisherPort
{
    private readonly IMediator _bus;

    public EventPublisherAdapter(IMediator bus)
    {
        _bus = bus;
    }

    public Task Publish(Event @event) => _bus.Publish(@event);
}