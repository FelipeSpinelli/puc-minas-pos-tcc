using ArquiveSe.Application.Models.Commands.Abstractions;
using System.Text.Json;

namespace ArquiveSe.Infra.Databases.Persistence.Models;

public class IdempotencyDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Key { get; set; } = null!;
    public string ValueType { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Data { get; set; } = string.Empty;

    public IdempotencyDto()
    {
    }

    public IdempotencyDto(
        string key,
        string valueType,
        object value)
    {
        Key = key;
        ValueType = valueType;
        Data = JsonSerializer.Serialize(value);
    }

    public T GetAs<T>()
    {
        var type = typeof(IIdempotencyCalculator).Assembly.GetType(ValueType)!;
        return (T)JsonSerializer.Deserialize(Data, type)!;
    }
}
