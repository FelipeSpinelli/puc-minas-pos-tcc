using ArquiveSe.Domain.Shared;

namespace ArquiveSe.Application.Ports.Driven;

public interface IPersistenceDbPort
{
    Task<T> LoadAggregate<T>(string aggregateId) where T : AggregateRoot;
    Task AddEvent(Event @event);
    Task SaveAndNotifyEvents();
}