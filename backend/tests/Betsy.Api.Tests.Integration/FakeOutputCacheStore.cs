using Microsoft.AspNetCore.OutputCaching;

namespace Betsy.Api.Tests.Integration;

public class FakeOutputCacheStore : IOutputCacheStore
{
    public ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
    {
        return new ValueTask(Task.CompletedTask);
    }

    public ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
    {
        byte[]? value = null;
        return new ValueTask<byte[]?>(Task.FromResult(value));
    }

    public ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
    {
        return new ValueTask(Task.CompletedTask);
    }
}
