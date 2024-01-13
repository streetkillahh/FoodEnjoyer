using FoodEnjoyerWPF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace FoodEnjoyerWPF.Domain.ViewModels
{
    
    public class dgProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        
        public Uri Image { get; set; }
    }
}
