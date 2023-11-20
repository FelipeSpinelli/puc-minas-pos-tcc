using ArquiveSe.Application.Ports.Driving;
using System.Text.Json;

namespace ArquiveSe.Infra.Messaging.Models;

public record QueueCommand
{
    public string CommandType { get; set; } = null!;
    public string CommandData { get; set; } = null!;

    public static QueueCommand CreateFrom<T>(T command)
    {
        return new()
        {
            CommandType = typeof(T).FullName ?? typeof(T).Name,
            CommandData = JsonSerializer.Serialize(command)
        };
    }

    public object GetCommand()
    {
        var type = typeof(ICommandBusPort).Assembly.GetType(CommandType)!;
        return JsonSerializer.Deserialize(CommandData, type)!;
    }
}
