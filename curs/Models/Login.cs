using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Login
    {
        public string ReturnUrl { get; set; }

        [Required(ErrorMessage ="Не указано имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Не указан пароль")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        
    }
}
