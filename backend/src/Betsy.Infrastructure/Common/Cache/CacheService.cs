using Betsy.Application.Common.Cache;
using Microsoft.AspNetCore.OutputCaching;

namespace Betsy.Infrastructure.Common.Cache;

public class CacheService : ICacheService
{
    private readonly IOutputCacheStore _outputCacheStore;

    public CacheService(IOutputCacheStore outputCacheStore)
    {
        _outputCacheStore = outputCacheStore;
    }

    public async Task InvalidateCache(string key, CancellationToken cancellationToken)
    {
        await _outputCacheStore.EvictByTagAsync(key, cancellationToken);
    }
}
