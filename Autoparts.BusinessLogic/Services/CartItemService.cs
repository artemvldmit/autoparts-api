using AutoMapper;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.DataAccess.Repositories.Interfaces;
using Autoparts.Domains.DTOs;
using Autoparts.Domains.Entities;

namespace Autoparts.BusinessLogic.Services;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _repo;
    private readonly IMapper _mapper;

    public CartItemService(ICartItemRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CartItemDto>> GetByUserAsync(int userId)
    {
        var items = await _repo.GetByUserAsync(userId);
        return _mapper.Map<IEnumerable<CartItemDto>>(items);
    }

    public async Task<CartItemDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdWithProductAsync(id);
        return item is null ? null : _mapper.Map<CartItemDto>(item);
    }

    public async Task<CartItemDto> CreateAsync(CreateCartItemDto dto)
    {
        var existing = await _repo.GetByUserAndProductAsync(dto.UserId, dto.ProductId);

        if (existing is not null)
        {
            existing.Quantity += dto.Quantity;
            await _repo.UpdateAsync(existing);
            var updated = await _repo.GetByIdWithProductAsync(existing.Id);
            return _mapper.Map<CartItemDto>(updated!);
        }

        var item = _mapper.Map<CartItem>(dto);
        await _repo.CreateAsync(item);
        return _mapper.Map<CartItemDto>(item);
    }

    public async Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto)
    {
        var item = await _repo.GetByIdWithProductAsync(id);
        if (item is null) return null;
        item.Quantity = dto.Quantity;
        await _repo.UpdateAsync(item);
        return _mapper.Map<CartItemDto>(item);
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _repo.DeleteAsync(id);
}
