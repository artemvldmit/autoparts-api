using Autoparts.Domains.DTOs;

namespace Autoparts.BusinessLogic.Interfaces;

public interface ICartItemService
{
    Task<IEnumerable<CartItemDto>> GetByUserAsync(int userId);
    Task<CartItemDto?> GetByIdAsync(int id);
    Task<CartItemDto> CreateAsync(CreateCartItemDto dto);
    Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto);
    Task<bool> DeleteAsync(int id);
}
