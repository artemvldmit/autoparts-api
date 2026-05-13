using Autoparts.Api.Filters;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.Domains.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoparts.Api.Controllers;

// User и Admin: заказы доступны только авторизованным
[Authorized]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service) => _service = service;

    // Admin: список всех заказов
    [AdminMod]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId) => Ok(await _service.GetByUserAsync(userId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    // Admin: только администратор может удалять заказы
    [AdminMod]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
