namespace OnlineShop.Db.Models
{
    public enum OrderStatuses
    {
        Created,
        Processed,
        Delivering,
        Delivered,
        Canceled
    }
    public class Order
    {
        public Guid Id { get; set; }
        public UserDeliveryInfo User { get; set; }
        public List<CartItem> Items { get; set; }
        public DateTime CreatedDateTime { get; set; } 
        public OrderStatuses Status { get; set; }
        public Order() 
        { 
           
            Status= OrderStatuses.Created;
            CreatedDateTime= DateTime.Now;

        }
    }
}
