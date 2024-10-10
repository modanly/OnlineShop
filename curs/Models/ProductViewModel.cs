using curs.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace curs.Models
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
        public string ImagePath { get; set; }

    }
}
