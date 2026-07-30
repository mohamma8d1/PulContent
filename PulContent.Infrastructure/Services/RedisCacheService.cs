using Microsoft.Extensions.Logging;
using PulContent.Application.Interfaces;
using StackExchange.Redis;

namespace PulContent.Infrastructure.Services;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : struct
    {
        if (!TryGetDatabase(out var db))
            return null;

        try
        {
            var value = await db.StringGetAsync(key);
            if (!value.HasValue)
                return null;

            return (T)Convert.ChangeType(value.ToString(), typeof(T));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Redis GET failed for key {Key}", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : struct
    {
        if (!TryGetDatabase(out var db))
            return;

        try
        {
            await db.StringSetAsync(key, value.ToString());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Redis SET failed for key {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (!TryGetDatabase(out var db))
            return;

        try
        {
            await db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Redis DEL failed for key {Key}", key);
        }
    }

    private bool TryGetDatabase(out IDatabase db)
    {
        if (!connectionMultiplexer.IsConnected)
        {
            logger.LogInformation("Redis not connected, skipping cache operation");
            db = null!;
            return false;
        }

        db = connectionMultiplexer.GetDatabase();
        return true;
    }
}
