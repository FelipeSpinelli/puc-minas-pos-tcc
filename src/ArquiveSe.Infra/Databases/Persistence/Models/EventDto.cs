using ArquiveSe.Domain.Shared;
using System.Text.Json;

namespace ArquiveSe.Infra.Databases.Persistence.Models;

public class EventDto
{
    public string Id { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public string AggregateType { get; set; } = null!;
    public string AggregateId { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Data { get; set; } = string.Empty;

    public EventDto()
    {
    }

    public EventDto(Event @event)
    {
        Id = @event.Id;
        EventType = @event.EventType;
        AggregateType = @event.AggregateType;
        AggregateId = @event.AggregateId;
        Timestamp = @event.Timestamp;
        Data = @event.GetData();
    }

    public Event GetEvent()
    {
        var eventType = typeof(Event).Assembly.GetType(EventType)!;
        return (Event)JsonSerializer.Deserialize(Data, eventType)!;
    }
}