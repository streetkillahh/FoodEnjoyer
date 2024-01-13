using FoodEnjoyerWPF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string FIO { get; set; }

        public byte Age { get; set; }

        public string Telephone { get; set; }

        public Role Role { get; set; }

        public List<Order> Orders { get; set; } = new();

        //public List<Relation> Relation { get; set; } = new();
        public List<Address> Addresses { get; set; } = new();
    }
}
