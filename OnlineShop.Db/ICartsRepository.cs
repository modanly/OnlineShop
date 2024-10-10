using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface ICartsRepository
    {
        void Add(Product product, string userId);
        void Clear(string userId);
        void DecreaseAmount(Guid productId, string userId);
        Cart TryGetByUserId(string userId);
    }
}