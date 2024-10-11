using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface ICartsRepository
    {
        Task AddAsync(Product product, string userId);
        Task ClearAsync(string userId);
        Task DecreaseAmountAsync(Guid productId, string userId);
        Task<Cart> TryGetByUserIdAsync(string userId);
    }
}