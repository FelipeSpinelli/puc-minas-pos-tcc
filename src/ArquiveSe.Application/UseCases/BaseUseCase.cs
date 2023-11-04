using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Shared;

namespace ArquiveSe.Application.UseCases;

public abstract class BaseUseCase
{
    protected readonly IPersistenceDbPort _persistenceDb;

    protected BaseUseCase(IPersistenceDbPort persistenceDb)
    {
        _persistenceDb = persistenceDb;
    }

    protected async Task PersistEventsOf(AggregateRoot aggregateRoot)
    {
        while (aggregateRoot!.TryGetEvent(out var @event))
        {
            await _persistenceDb.AddEvent(@event);
        }

        await _persistenceDb.SaveAndNotifyEvents();
    }
}
