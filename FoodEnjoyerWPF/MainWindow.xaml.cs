using FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories;
using FoodEnjoyerWPF.FoodEnjoyer.DAL;
using FoodEnjoyerWPF.Service.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoodEnjoyerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        public MainWindow()
        {
            InitializeComponent();

            var _db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
            _userService = new UserService(new UserRepository(_db));

            MainFrame.NavigationService.Navigate(new Pages.EnterPage(_userService, _db));
        }
    }
}
