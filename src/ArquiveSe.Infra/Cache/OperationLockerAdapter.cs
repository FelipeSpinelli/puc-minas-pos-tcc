using ArquiveSe.Application.Ports.Driven;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;

namespace ArquiveSe.Infra.Cache;

public class OperationLockerAdapter : IOperationLockerPort
{
    private readonly IDistributedCache _cache;
    private static readonly Mutex _mutex = new();

    public OperationLockerAdapter(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<bool> TryLock(string key)
    {
        const int TIMEOUT_IN_SECONDS = 30;
        const int WAIT_IN_MILLISECONDS = 200;

        var sw = Stopwatch.StartNew();
        bool wasAlreadyCached;

        do
        {
            if (sw.Elapsed.TotalSeconds >= TIMEOUT_IN_SECONDS)
            {
                wasAlreadyCached = true;
                break;
            }

            (wasAlreadyCached, _) = await GetOrAddFromCache(key);
            await Task.Delay(!wasAlreadyCached ? WAIT_IN_MILLISECONDS : 0);
        }
        while (wasAlreadyCached);
        
        return !wasAlreadyCached;
    }

    public Task Unlock(string key) => _cache.RemoveAsync(key);

    private async Task<(bool WasAlreadyCached, string Value)> GetOrAddFromCache(string key)
    {
        _mutex.WaitOne();
        var value = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            _mutex.ReleaseMutex();
            return (true, value);
        }

        await _cache.SetStringAsync(
            key, 
            Guid.NewGuid().ToString(), 
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)}
        );
        _mutex.ReleaseMutex();
        return (false, value);
    }
}
