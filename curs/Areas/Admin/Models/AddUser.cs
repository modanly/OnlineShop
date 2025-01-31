﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.Models
{
    public class AddUser
    {
        [Required(ErrorMessage = "Не указано имя пользователя")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Имя пользователя должно содержать от 2 до 15 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Пароль должен содержать от 8 до 30 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан повторный пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        /*
                [Required(ErrorMessage = "Не указано имя пользователя")]
                [StringLength(200, MinimumLength = 1, ErrorMessage = "Имя пользователя должно содержать от 1 до 200 символов")]
                public string FirstName { get; set; }

                [Required(ErrorMessage = "Не указана фамилия пользователя")]
                [StringLength(200, MinimumLength = 1, ErrorMessage = "Фамилия пользователя должно содержать от 1 до 200 символов")]
                public string LastName { get; set; }
        */
        [Required(ErrorMessage = "Не указан телефон пользователя")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Телефон пользователя должно содержать от 5 до 50 символов")]
        public string Phone { get; set; }
    }
}
