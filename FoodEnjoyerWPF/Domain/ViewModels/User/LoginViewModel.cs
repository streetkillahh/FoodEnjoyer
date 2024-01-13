using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.Domain.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [MaxLength(20, ErrorMessage = "Логин должен иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Логин должен иметь длину больше 3 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину больше 6 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

    }
}
