using Autoparts.BusinessLogic.Interfaces;
using Autoparts.Domains.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoparts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartItemsController : ControllerBase
{
    private readonly ICartItemService _service;

    public CartItemsController(ICartItemService service) => _service = service;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId) => Ok(await _service.GetByUserAsync(userId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCartItemDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCartItemDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
