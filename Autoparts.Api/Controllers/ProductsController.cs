using Autoparts.Api.Filters;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.Domains.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Autoparts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service) => _service = service;

    // Visitor: доступно всем
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? category)
    {
        if (!string.IsNullOrEmpty(category))
            return Ok(await _service.GetByCategoryAsync(category));
        return Ok(await _service.GetAllAsync());
    }

    // Visitor: доступно всем
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    // Admin: только администратор может добавлять товары
    [AdminMod]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Admin: только администратор может редактировать товары
    [AdminMod]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    // Admin: только администратор может удалять товары
    [AdminMod]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
