namespace MockStocks.Core.Tests;

public sealed class LRUCacheTests
{
    [Fact]
    public void LRUCache_Get()
    {
        var cache = new LRUCache<int, int>(2);
        
        cache.Put(1, 1);
        cache.Put(2, 2);
        Assert.Equal(1, cache.Get(1));
        
        cache.Put(3, 3);
        Assert.Throws<KeyNotFoundException>(() => cache.Get(2));

        cache.Put(4, 4);
        Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        Assert.Equal(3, cache.Get(3));
        Assert.Equal(4, cache.Get(4));
    }
}