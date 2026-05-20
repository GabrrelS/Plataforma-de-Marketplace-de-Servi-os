using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class ProductCacheService
{
    private readonly IDistributedCache _cache;

    public ProductCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }

    public async Task SetAsync(string key, object value)
    {
        var json = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}