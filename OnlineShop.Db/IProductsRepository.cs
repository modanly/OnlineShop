using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IProductsRepository
    {
        List<Product> GetAll();
        Product TryGetById(Guid id);
        void Add(Product product);
        void Del(Guid id);
        void Update(Product product);
    }
}