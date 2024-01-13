using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.Domain.ViewModels.Address
{
    public class AddressViewModel
    {
        [Display(Name = "Улица")]
        [Required(ErrorMessage = "Укажите улицу")]
        [MinLength(1, ErrorMessage = "Улица должна быть больше 1 символа")]
        [MaxLength(50, ErrorMessage = "Улица должна быть меньше 50 символов")]
        public string Street { get; set; }

        [Display(Name = "Дом")]
        [Required(ErrorMessage = "Укажите дом")]
        [MinLength(1, ErrorMessage = "Дом должен быть больше 1 символа")]
        [MaxLength(10, ErrorMessage = "Дом должен быть меньше 10 символов")]
        public string Home { get; set; }

        [Display(Name = "Квартира")]
        [Required(ErrorMessage = "Укажите квартиру")]
        public int Apartment { get; set; }

        [Display(Name = "Подъезд")]
        [Required(ErrorMessage = "Укажите Подъезд")]
        public int Entrance { get; set; }
    }
}
