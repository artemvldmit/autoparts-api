using Autoparts.Domains.Entities;

namespace Autoparts.DataAccess.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetAllWithItemsAsync();
    Task<Order?> GetByIdWithItemsAsync(int id);
    Task<IEnumerable<Order>> GetByUserAsync(int userId);
}
