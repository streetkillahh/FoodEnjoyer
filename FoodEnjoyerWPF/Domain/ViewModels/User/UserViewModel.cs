using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.Domain.ViewModels.User
{
    public class UserViewModel
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        

        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите ФИО")]
        [MaxLength(100, ErrorMessage = "ФИО должно иметь длину меньше 100 символов")]
        [MinLength(5, ErrorMessage = "ФИО должно иметь длину больше 5 символов")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Укажите возраст")]
        [Range(0, 150, ErrorMessage = "Диапазон возраста должен быть от 0 до 150")]
        public byte Age { get; set; }

        [MaxLength(11)]
        [Required(ErrorMessage = "Укажите Телефон")]
        [Display(Name = "Пароль")]
        public string Telephone { get; set; }


        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public string Role { get; set; }

        public List<FoodEnjoyerWPF.Domain.Entity.Order> Orders { get; set; }  


    }
}
