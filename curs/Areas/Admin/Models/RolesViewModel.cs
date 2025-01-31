﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.Models
{
    public class RolesViewModel
    {
        [Required(ErrorMessage = "Не указано имя роли")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Имя роли должно содержать от 1 до 50 символов")]
        public string Name { get; set; }

        public RolesViewModel() { }
        public RolesViewModel(string name)
        {
            Name = name;
        }
       
    }
}
