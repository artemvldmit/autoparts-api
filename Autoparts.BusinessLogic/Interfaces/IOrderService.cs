using Autoparts.Domains.DTOs;

namespace Autoparts.BusinessLogic.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetByUserAsync(int userId);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto);
    Task<bool> DeleteAsync(int id);
}
