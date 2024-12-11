using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.Models
{
    public class EditProductViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 1000000, ErrorMessage = "Цена должна быть от 1 до 1000000 руб.")]
        public decimal Cost { get; set; }
        [Required]
        public string Description { get; set; }
        public List<string> ImagesPaths { get; set; } = new List<string>();
        public IFormFile[]? UploadedFiles { get; set; }
       
       
    }
}
