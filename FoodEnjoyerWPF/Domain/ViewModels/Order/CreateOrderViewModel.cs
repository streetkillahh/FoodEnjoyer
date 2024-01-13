using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FoodEnjoyerWPF.Domain.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage ="Вы не указали количество ")]
        [Range(1, 5, ErrorMessage = "Количество должно быть от 1 до 5")]
        public int Quantity { get; set; }

        [Display(Name = "Сумма")]
        [Required(ErrorMessage = "Отстутствует сумма покупки")]
        public double Sum { get; set; }

        [Required(ErrorMessage = "Укажите способ оплаты")]
        [Display(Name = "Способ оплаты")]
        public string PayMethod { get; set; }

        [Required(ErrorMessage = "Укажите способ получения")]
        [Display(Name = "Способ получения")]
        public string GetMethod { get; set; }
        [Required(ErrorMessage = "Выберите адрес доставки")]
        [Display (Name ="Адрес")]
        public int AddressId { get; set; }


        public bool Status { get; set; }

        [Display(Name = "Комментарий")]
        [MaxLength(80, ErrorMessage = "Комментарий должен иметь длину меньше 80 символов")]
        public string Comment { get; set; }

        public string Login { get; set; }

        public List<Domain.Entity.Product> Products { get; set; }



        //PayMethod
        //GetMethod
        //Status
        //User
        //Products

    }
}
