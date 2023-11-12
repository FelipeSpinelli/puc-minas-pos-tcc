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
        _mutex.WaitOne();
        bool wasAlreadyCached;

        do
        {
            if (sw.ElapsedMilliseconds * 1000 >= TIMEOUT_IN_SECONDS)
            {
                _mutex.ReleaseMutex();
                return false;
            }

            (wasAlreadyCached, _) = await GetOrAddFromCache(key);
            await Task.Delay(!wasAlreadyCached ? WAIT_IN_MILLISECONDS : 0);
        }
        while (!wasAlreadyCached);
        
        _mutex.ReleaseMutex();
        return !wasAlreadyCached;
    }

    public Task Unlock(string key) => _cache.RemoveAsync(key);

    private async Task<(bool WasAlreadyCached, string Value)> GetOrAddFromCache(string key)
    {
        var value = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            return (true, value);
        }

        await _cache.SetStringAsync(
            key, 
            Guid.NewGuid().ToString(), 
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)}
        );
        return (false, value);
    }
}
