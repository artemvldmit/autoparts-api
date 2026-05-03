using Autoparts.DataAccess.Repositories.Interfaces;
using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.DataAccess.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext _db;

    public CartItemRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<CartItem>> GetAllAsync() =>
        await _db.CartItems.Include(c => c.Product).ToListAsync();

    public async Task<CartItem?> GetByIdAsync(int id) =>
        await _db.CartItems.FindAsync(id);

    public async Task<CartItem?> GetByIdWithProductAsync(int id) =>
        await _db.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<CartItem>> GetByUserAsync(int userId) =>
        await _db.CartItems.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync();

    public async Task<CartItem?> GetByUserAndProductAsync(int userId, int productId) =>
        await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

    public async Task<CartItem> CreateAsync(CartItem entity)
    {
        _db.CartItems.Add(entity);
        await _db.SaveChangesAsync();
        await _db.Entry(entity).Reference(c => c.Product).LoadAsync();
        return entity;
    }

    public async Task UpdateAsync(CartItem entity)
    {
        _db.CartItems.Update(entity);
        await _db.SaveChangesAsync();
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
