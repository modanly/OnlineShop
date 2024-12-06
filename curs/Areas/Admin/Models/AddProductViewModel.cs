using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.Models
{
    public class AddProductViewModel
    {
        
        [Required]
        public string Name { get; set; }
        [Range(1, 1000000, ErrorMessage = "Цена должна быть от 1 до 1000000 руб.")]
        public decimal Cost { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile[] UploadedFiles { get; set; }
    }
}
