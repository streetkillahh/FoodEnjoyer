using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.ViewModels
{
    public class dgOrderVM
    {
        public int IdOrder {get; set; }
        public string UserOfOrder {get; set; }
        public string UserPayMethod {get; set; }
        public string UserGetMethod {get; set; }
        public string DateOrder {get; set; }
        public string AddressOrder {get; set; }
        public int CountFood {get; set; }
        public double AllPrice {get; set; }
        public string Commentary { get; set; }

    }
}
