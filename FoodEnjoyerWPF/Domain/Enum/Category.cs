using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.Domain.Enum
{
    public enum Category
    {
        [Display(Name = "Напитки")]
        Супы = 0,
        [Display(Name = "Горячие блюда")]
        Горячие_блюда = 1,
        [Display(Name = "Закуски")]
        Закуски = 2,
        [Display(Name = "Десерты")]
        Десерты = 3,
        [Display(Name = "Напитки")]
        Напитки = 4,

    }
}
