using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Infra.Databases.Persistence.Configurations;
using ArquiveSe.Infra.Databases.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace ArquiveSe.Infra.Databases.Persistence;

public class PersistenceDbAdapter : IPersistenceDbPort
{
    private readonly PersistenceDbContext _db;
    private readonly IEventPublisherPort _eventPublisher;
    private readonly Queue<Event> _events = new();

    public PersistenceDbAdapter(PersistenceDbContext db, IEventPublisherPort eventPublisher)
    {
        _db = db;
        _eventPublisher = eventPublisher;
    }

    public async Task AddEvent(Event @event)
    {
        _events.Enqueue(@event);
        await _db.Events.AddAsync(new EventDto(@event));
    }

    public async Task<T> LoadAggregate<T>(string aggregateId)
        where T : AggregateRoot
    {
        var aggregateType = typeof(T);
        var aggregateEvents = await _db.Events
            .Where(x => x.AggregateId == aggregateId)
            .ToListAsync();

        var aggregate = (T)Activator.CreateInstance(aggregateType)!;

        foreach (var @event in aggregateEvents)
        {
            aggregate.ApplyEvent(@event.GetEvent());
        }

        return aggregate;
    }

    public async Task SaveAndNotifyEvents()
    {
        await _db.SaveChangesAsync();

        while(_events.TryDequeue(out var @event))
        {
            await _eventPublisher.Publish(@event);
        }
    }
}
