using Microsoft.AspNetCore.Mvc;
using MockStocks.Core;
using MockStocks.Web.Models;

namespace MockStocks.Web.Controllers;

/// <summary>
/// Handles HTTP requests for stocks.
/// </summary>
[ApiController]
[Route("stocks")]
public sealed class StocksController : ControllerBase
{
    private readonly IPriceHistoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="StocksController"/> class.
    /// </summary>
    /// <param name="cache">
    /// An instance of <see cref="IPriceHistoryCache"/>.
    /// </param>
    public StocksController(IPriceHistoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Handles the HTTP request to get the price history of a stock.
    /// </summary>
    /// <param name="symbol">
    /// The symbol of the stock to retrieve.
    /// </param>
    /// <returns>
    /// The price history of the stock.
    /// </returns>
    [HttpGet("{symbol:alpha}")]
    public Task<IActionResult> GetPriceHistory([FromRoute] string symbol)
    {
        symbol = symbol.ToLower();

        List<decimal> prices;
        try
        {
            prices = new(_cache.GetPriceHistory(symbol));
        }
        catch (KeyNotFoundException)
        {
            prices = GeneratePriceHistory(50);
            _cache.PutPriceHistory(symbol, prices);
        }

        return Task.FromResult<IActionResult>(Ok(new PriceHistoryResponse
        {
            Symbol = symbol,
            History = prices,
        }));
    }

    private static List<decimal> GeneratePriceHistory(int days)
    {
        // Generate price history from a random starting price.
        var price = (decimal)Random.Shared.Next(5, 1000);
        var prices = new List<decimal>();
        
        for (var i = 0; i < days; i++)
        {
            // Calculate random percent change between -5% and 5%.
            var percentChange = 1.00m + Random.Shared.Next(-5, 6) / 100m;

            // Calculate the new price.
            price *= percentChange;
            prices.Add(price);
        }

        return prices;
    }
}