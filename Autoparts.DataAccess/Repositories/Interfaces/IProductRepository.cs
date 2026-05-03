using Autoparts.Domains.Entities;

namespace Autoparts.DataAccess.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
}
