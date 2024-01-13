using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.Entity
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string? FIO { get; set; }

        public byte Age { get; set; }


        public string Address { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }
    }
}
