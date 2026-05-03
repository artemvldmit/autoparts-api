using AutoMapper;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.DataAccess;
using Autoparts.Domains.DTOs;
using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.BusinessLogic.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);
        return order is null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetByUserAsync(int userId)
    {
        var orders = await _db.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order { UserId = dto.UserId };

        foreach (var item in dto.OrderItems)
        {
            var product = await _db.Products.FindAsync(item.ProductId);
            if (product is null) continue;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        await _db.Entry(order).Collection(o => o.OrderItems).Query().Include(oi => oi.Product).LoadAsync();
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto)
    {
        var order = await _db.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return null;
        order.Status = dto.Status;
        await _db.SaveChangesAsync();
        return _mapper.Map<OrderDto>(order);
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
