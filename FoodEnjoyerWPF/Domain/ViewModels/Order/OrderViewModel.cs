using FoodEnjoyerWPF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public string DateCreate {get; set;} 
        public int Quantity {get; set;} 
        public double Sum {get; set;} 
        public string PayMethod {get; set;} 
        public string GetMethod {get; set;} 
        public string Comment {get; set;} 
        public string Address { get; set; } 
        public string Status {get; set;} 

    }
}
