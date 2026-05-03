using AutoMapper;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.DataAccess;
using Autoparts.Domains.DTOs;
using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.BusinessLogic.Services;

public class CartItemService : ICartItemService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public CartItemService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CartItemDto>> GetByUserAsync(int userId)
    {
        var items = await _db.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CartItemDto>>(items);
    }

    public async Task<CartItemDto?> GetByIdAsync(int id)
    {
        var item = await _db.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == id);
        return item is null ? null : _mapper.Map<CartItemDto>(item);
    }

    public async Task<CartItemDto> CreateAsync(CreateCartItemDto dto)
    {
        var existing = await _db.CartItems
            .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.ProductId == dto.ProductId);

        if (existing is not null)
        {
            existing.Quantity += dto.Quantity;
            await _db.SaveChangesAsync();
            await _db.Entry(existing).Reference(c => c.Product).LoadAsync();
            return _mapper.Map<CartItemDto>(existing);
        }

        var item = _mapper.Map<CartItem>(dto);
        _db.CartItems.Add(item);
        await _db.SaveChangesAsync();
        await _db.Entry(item).Reference(c => c.Product).LoadAsync();
        return _mapper.Map<CartItemDto>(item);
    }

    public async Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto)
    {
        var item = await _db.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == id);
        if (item is null) return null;
        item.Quantity = dto.Quantity;
        await _db.SaveChangesAsync();
        return _mapper.Map<CartItemDto>(item);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.CartItems.FindAsync(id);
        if (item is null) return false;
        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}
