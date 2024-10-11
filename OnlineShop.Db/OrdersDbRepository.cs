using Microsoft.EntityFrameworkCore;

using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public class OrdersDbRepository : IOrdersRepository
    {
        private readonly DatabaseContext databaseContext;
        public OrdersDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        
        public async Task AddAsync(Order order)
        {
            databaseContext.Orders.Add(order);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await databaseContext.Orders.Include(x => x.User)
                .Include(x =>x.Items).ThenInclude(x =>x.Product).ToListAsync();
        }

        public async Task<Order> TryGetByIdAsync(Guid id)
        {
            return await databaseContext.Orders.Include(x=>x.User).Include(x=>x.Items).ThenInclude(x=>x.Product).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task UpdateStatusAsync(Guid orderId, OrderStatuses newStatus)
        {
            var order=await TryGetByIdAsync(orderId);
            if(order != null)
            {
                order.Status = newStatus;
            }
            await databaseContext.SaveChangesAsync();
        }
    }
}
