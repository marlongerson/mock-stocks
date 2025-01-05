using MockStocks.Web.Controllers;

namespace MockStocks.Web.Models;

/// <summary>
/// Represents the response to a <see cref="StocksController.GetPriceHistory"/> request.
/// </summary>
public sealed class PriceHistoryResponse
{
    /// <summary>
    /// The stock symbol.
    /// </summary>
    public required string Symbol { get; set; }

    /// <summary>
    /// The stock price history.
    /// </summary>
    public required IEnumerable<decimal> History { get; set; }
}