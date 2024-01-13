using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.Entity
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public int Apartment { get; set; }
        public byte Entrance { get; set; }

        public List<Order> Orders { get; set; }
       // public List<Relation> Relation { get; set; } = new();
        public List<User> Users { get; set; } = new();

    }
}
