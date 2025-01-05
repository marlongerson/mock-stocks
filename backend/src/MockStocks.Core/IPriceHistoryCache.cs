namespace MockStocks.Core;

/// <summary>
/// Represents a cache for stock price history.
/// </summary>
public interface IPriceHistoryCache
{
    /// <summary>
    /// Gets the price history of the specified symbol.
    /// </summary>
    /// <param name="symbol">
    /// The symbol to get the price history.
    /// </param>
    /// <returns>
    /// The price history of the stock.
    /// </returns>
    public IEnumerable<decimal> GetPriceHistory(string symbol);
    
    /// <summary>
    /// Puts the price history of a stock into the cache.
    /// </summary>
    /// <param name="symbol">
    /// The symbol of the stock.
    /// </param>
    /// <param name="prices">
    /// The price history of the stock.
    /// </param>
    public void PutPriceHistory(string symbol, IEnumerable<decimal> prices);
}