using StackExchange.Redis;
using System.Text.Json;

namespace RedditStats.Services;

public class CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger) : ICacheService
{
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var db = redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiry);
        logger.LogInformation($"Cached data with key: {key}");
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var db = redis.GetDatabase();
        var json = await db.StringGetAsync(key);

        if (json.IsNullOrEmpty)
        {
            logger.LogWarning($"Cache miss for key: {key}");
            return default;
        }
        
        logger.LogInformation($"Cache hit for key: {key}");
        return JsonSerializer.Deserialize<T>(json);
    }
}
