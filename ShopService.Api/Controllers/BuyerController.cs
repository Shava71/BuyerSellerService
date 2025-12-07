using Microsoft.AspNetCore.Mvc;
using ShopService.Api.Contracts;
using ShopService.Application.Services;
using ShopService.Application.Services.Implementations;
using ShopService.Application.Services.Interfaces;
using ShopService.Infrastructure.Stats;

namespace ShopService.Api.Controllers;

[ApiController]
[Route("api/buyer")]
public class BuyerController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;
    private readonly IStatsService _statsService;

    public BuyerController(IPurchaseService purchaseService, IStatsService statsService )
    {
        _purchaseService = purchaseService;
        _statsService = statsService;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody]PurchaseRequest request)
    {
        try
        {
            await _purchaseService.PurchaseAsync(request.Ids);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        try
        {
            StatsConfig stats = await _statsService.GetStatsAsync();
            return Ok(stats);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}