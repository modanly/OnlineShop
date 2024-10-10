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

        
        public void Add(Order order)
        {
            databaseContext.Orders.Add(order);
            databaseContext.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            return databaseContext.Orders.Include(x => x.User)
                .Include(x =>x.Items).ThenInclude(x =>x.Product).ToList();
        }

        public Order TryGetById(Guid id)
        {
            return databaseContext.Orders.Include(x=>x.User).Include(x=>x.Items).ThenInclude(x=>x.Product).FirstOrDefault(x=>x.Id==id);
        }

        public void UpdateStatus(Guid orderId, OrderStatuses newStatus)
        {
            var order=TryGetById(orderId);
            if(order != null)
            {
                order.Status = newStatus;
            }
            databaseContext.SaveChanges();
        }
    }
}
