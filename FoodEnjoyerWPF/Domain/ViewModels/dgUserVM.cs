using FoodEnjoyerWPF.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FoodEnjoyerWPF.Domain.ViewModels
{
    public class dgUserVM
    {
        public int IdUser {get; set;}
        public string Role {get; set;}
        public string UserLogin {get; set;}
        public string UserTelephone {get; set;}
        public string UserFIO {get; set;}
        public int UserAge {get; set;}
        public double TotalCost {get; set;}
        public int TotalOrders { get; set; }

    }
}
