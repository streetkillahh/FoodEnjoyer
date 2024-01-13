using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.Enum
{
    public enum GetMethod
    {
        [Display(Name = "Зал")]
        Зал = 0,
        [Display(Name = "Доставка")]
        Доставка = 1,
    }
}
