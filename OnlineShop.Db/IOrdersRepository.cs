using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IOrdersRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> TryGetByIdAsync(Guid id);
        Task UpdateStatusAsync(Guid orderId, OrderStatuses status);
    }
}