using MockStocks.Core;

namespace MockStocks.Infrastructure;

public sealed class InMemoryPriceHistoryCache : IPriceHistoryCache
{
    private readonly LRUCache<string, IEnumerable<decimal>> _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryPriceHistoryCache"/> class.
    /// </summary>
    public InMemoryPriceHistoryCache(int capacity)
    {
        _cache = new LRUCache<string, IEnumerable<decimal>>(capacity);
    }

    /// <inheritdoc/>
    public IEnumerable<decimal> GetPriceHistory(string symbol)
    {
        return _cache.Get(symbol);
    }

    /// <inheritdoc/>
    public void PutPriceHistory(string symbol, IEnumerable<decimal> prices)
    {
        _cache.Put(symbol, prices);
    }
}