using AutoMapper;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.DataAccess.Repositories.Interfaces;
using Autoparts.Domains.DTOs;
using Autoparts.Domains.Entities;

namespace Autoparts.BusinessLogic.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepo, IProductRepository productRepo, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepo.GetAllWithItemsAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepo.GetByIdWithItemsAsync(id);
        return order is null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetByUserAsync(int userId)
    {
        var orders = await _orderRepo.GetByUserAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order { UserId = dto.UserId };

        foreach (var item in dto.OrderItems)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product is null) continue;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
        await _orderRepo.CreateAsync(order);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto)
    {
        var order = await _orderRepo.GetByIdWithItemsAsync(id);
        if (order is null) return null;
        order.Status = dto.Status;
        await _orderRepo.UpdateAsync(order);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _orderRepo.DeleteAsync(id);
}
