using Autoparts.DataAccess.Repositories.Interfaces;
using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _db.Orders.ToListAsync();

    public async Task<IEnumerable<Order>> GetAllWithItemsAsync() =>
        await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();

    public async Task<Order?> GetByIdAsync(int id) =>
        await _db.Orders.FindAsync(id);

    public async Task<Order?> GetByIdWithItemsAsync(int id) =>
        await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetByUserAsync(int userId) =>
        await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).Where(o => o.UserId == userId).ToListAsync();

    public async Task<Order> CreateAsync(Order entity)
    {
        _db.Orders.Add(entity);
        await _db.SaveChangesAsync();
        await _db.Entry(entity).Collection(o => o.OrderItems).Query().Include(oi => oi.Product).LoadAsync();
        return entity;
    }

    public async Task UpdateAsync(Order entity)
    {
        _db.Orders.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null) return false;
        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }
}
