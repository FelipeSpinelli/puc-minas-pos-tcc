using ArquiveSe.Domain.Shared;

namespace ArquiveSe.Application.Ports.Driven;

public interface IEventPublisherPort
{
    Task Publish(Event @event);
}
