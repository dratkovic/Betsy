using Betsy.Application.Common.Cache;

namespace Betsy.Api.Tests.Integration;

public class FakeCacheService : ICacheService
{
    public Task InvalidateCache(string key, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
