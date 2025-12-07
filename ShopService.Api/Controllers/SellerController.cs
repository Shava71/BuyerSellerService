using Microsoft.AspNetCore.Mvc;
using ShopService.Application.Dtos;
using ShopService.Application.Services;
using ShopService.Application.Services.Implementations;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Entities;

namespace ShopService.Api.Controllers;

/// <summary>
/// CRUD
/// </summary>
[ApiController]
[Route("api/seller")]
public class SellerController : ControllerBase
{
    private readonly ISellerService _seller;

    public SellerController(ISellerService seller)
    {
        _seller = seller;
    }
    // R
    [HttpGet("items")]
    public async Task<IActionResult> GetAll() => Ok(await _seller.GetAllAsync());
    
    // R
    [HttpGet("items/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        Item? it = await _seller.GetByIdAsync(id);
        return it == null ? NotFound() : Ok(it);
    }
    // C
    [HttpPost("items/create")]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        Item created = await _seller.CreateAsync(dto);
        return Ok(created);
    }
    
    // U
    [HttpPut("items/update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateItemDto dto)
    {
        if (id != dto.Id) return BadRequest();
        await _seller.UpdateAsync(dto);
        return Ok($"item_{id} was updated");
    }
    
    // D
    [HttpDelete("items/delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _seller.DeleteAsync(id);
        return Ok($"item_{id} was deleted");
    }
}