using ArquiveSe.Domain.Shared;

namespace ArquiveSe.Application.Ports.Driven;

public interface IPersistenceDbPort
{
    Task<T> LoadAggregate<T>(string aggregateId);
    Task AddEvent(Event @event);
    Task SaveAndNotifyEvents();
    Task<ushort> GetNextDocumentSequentialToFolder(string folderId);
}