namespace ArquiveSe.Application.Ports.Driving;

public interface ICommandBusPort
{
    Task Send<T>(T message);
    Task Run<T>(T message);
}
