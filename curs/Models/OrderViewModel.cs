using curs.Models;

namespace curs.Models
{
    public enum OrderStatusesViewModel
    {
        Created,
        Processed,
        Delivering,
        Delivered,
        Canceled
    }
    public class OrderViewModel
    {
    

        public Guid Id { get; set; }
        public UserDeliveryInfoViewModel User { get; set; }
        public List<CartItemViewModel> Items { get; set; }
        public decimal Cost
        {
            get
            {
                return Items.Sum(x => x.Cost);
            }
        }
       
        public DateTime CreatedDateTime { get; set; }
        public OrderStatusesViewModel Status { get; set; }
        public OrderViewModel() 
        { 
           // Id = Guid.NewGuid();
            CreatedDateTime= DateTime.Now;
            Status= OrderStatusesViewModel.Created;
        }
    }
}
