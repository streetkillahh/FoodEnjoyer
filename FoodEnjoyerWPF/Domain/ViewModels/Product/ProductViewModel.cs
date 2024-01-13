using FoodEnjoyerWPF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.Domain.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id {get; set;}
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите Название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше двух символов")]
        public string Name {get; set;}

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Укажите категорию")]
        public string Category {get; set;}

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Введите Описание")]
        [MinLength(50, ErrorMessage = "Минимальная длина должна быть больше 50 символов")]
        public string Description {get; set;}
        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Укажите стоимость")]
        [Range(5, int.MaxValue, ErrorMessage ="цена должна быть от 5 до 2кк")]
        public double Price {get; set;}
        public int Count { get; set; }

        [Required(ErrorMessage = "Выберите изображение")]
        public string? ImgSource { get; set; }

    }
}
