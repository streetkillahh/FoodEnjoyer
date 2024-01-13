using FoodEnjoyerWPF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.Entity
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreate { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public double Sum { get; set; }
        public PayMethod PayMethod { get; set; }
        public GetMethod GetMethod { get; set; }
        public bool Status { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }


        public int AddressId { get; set; }
        public Address Address { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
