using MockStocks.Core;
using MockStocks.Infrastructure;

namespace MockStocks.Web;

/// <summary>
/// Provides the main entry point to the application.
/// </summary>
public class Program
{
    private const int DefaultLRUCacheCapacity = 50;
    
    /// <summary>
    /// The main entry point to the application.
    /// </summary>
    /// <param name="args">
    /// The command line arguments.
    /// </param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IPriceHistoryCache, InMemoryPriceHistoryCache>(
            _ => new InMemoryPriceHistoryCache(int.Parse(builder.Configuration["LRUCache:Capacity"] ?? DefaultLRUCacheCapacity.ToString())));
        
        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}