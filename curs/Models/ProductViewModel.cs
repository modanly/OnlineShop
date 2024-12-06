using OnlineShop.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace OnlineShop.Models
{
    public class ProductViewModel
    {
      
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1,1000000, ErrorMessage ="Цена должна быть от 1 до 1000000 руб.")]
        public decimal Cost { get; set; }
        [Required]
        public string Description { get; set; }
        public List<string> ImagesPaths { get; set; }
        public string ImagePath => ImagesPaths.Count == 0 ? "/images/Products/image3.jpeg" : ImagesPaths[0];
    }
}
