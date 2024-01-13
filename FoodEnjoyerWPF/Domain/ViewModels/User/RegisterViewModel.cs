using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [MaxLength(20, ErrorMessage = "Логин должен иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Логин должен иметь длину больше 3 символов")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину больше 6 символов")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Укажите ФИО")]
        [MaxLength(100, ErrorMessage = "ФИО должно иметь длину меньше 100 символов")]
        [MinLength(5, ErrorMessage = "ФИО должно иметь длину больше 5 символов")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Укажите возраст")]
        [Range(1, 150, ErrorMessage = "Диапазон возраста должен быть от 1 до 150")]
        public byte Age { get; set; }

        [MinLength(11, ErrorMessage = "Номер телефона должен содержать 11 символов")]
        [MaxLength(11, ErrorMessage = "Номер телефона должен содержать 11 символов")]
        [Required(ErrorMessage = "Укажите Телефон")]
        [Display(Name = "Пароль")]
        public string Telephone { get; set; }


        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public int Role { get; set; }


    }
}
