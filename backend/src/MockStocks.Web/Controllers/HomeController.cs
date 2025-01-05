using Microsoft.AspNetCore.Mvc;

namespace MockStocks.Web.Controllers;

/// <summary>
/// Provides an endpoint for the index.
/// </summary>
[ApiController]
[Route("/")]
public sealed class HomeController : ControllerBase
{
    /// <summary>
    /// Handles the HTTP request for the index.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(Ok("Welcome"));
    }
}