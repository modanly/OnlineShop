using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Db.Models
{
    public class Product
    {
     
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public List<Image> Images { get; set; }

        [Timestamp]
        public byte[] ConcurrencyToken { get; set; }
        public List<CartItem> CartItems { get; set; }
       public Product() 
        {
            CartItems = new List<CartItem>();
            Images = new List<Image>();
        }
        

        public Product(Guid id, string name, decimal cost, string description):this()
        {
            Id= id;
            Name = name;
            Cost = cost;
            Description = description;
            
        }
      

    }
}
