using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.Domain.Enum
{
    public enum PayMethod
    {
        [Display(Name = "Картой")]
        Картой = 0,
        [Display(Name = "Наличными")]
        Наличными = 1,
    }
}
