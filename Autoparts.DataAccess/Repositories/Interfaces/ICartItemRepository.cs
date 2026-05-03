using Autoparts.Domains.Entities;

namespace Autoparts.DataAccess.Repositories.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<IEnumerable<CartItem>> GetByUserAsync(int userId);
    Task<CartItem?> GetByUserAndProductAsync(int userId, int productId);
    Task<CartItem?> GetByIdWithProductAsync(int id);
}
