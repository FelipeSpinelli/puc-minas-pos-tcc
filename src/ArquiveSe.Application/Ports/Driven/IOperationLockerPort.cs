namespace ArquiveSe.Application.Ports.Driven;

public interface IOperationLockerPort
{
    Task<bool> TryLock(string key);
    Task Unlock(string key);
}