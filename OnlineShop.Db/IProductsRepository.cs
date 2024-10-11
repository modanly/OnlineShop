using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> TryGetByIdAsync(Guid id);
        Task AddAsync(Product product);
        Task DelAsync(Guid id);
        Task UpdateAsync(Product product);
    }
}