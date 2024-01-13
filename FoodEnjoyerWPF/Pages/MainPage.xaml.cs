using FoodEnjoyerWPF.DAL.Repositories;
using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.ViewModels;
using FoodEnjoyerWPF.FoodEnjoyer.DAL;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories;
using FoodEnjoyerWPF.Service.Implementations;
using FoodEnjoyerWPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

namespace FoodEnjoyerWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _db;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly AddressService _addressService;

        public MainPage(UserService userService, ApplicationDbContext db)
        {
            InitializeComponent();
            _userService = userService;
            _db = db;
            _productService = new ProductService(new ProductRepository(_db));         //это звиздец
            _orderService = new OrderService(new ProductRepository(db), new UserRepository(db), new OrderRepository(_db));

            this.Loaded += (s, e) => FillUserOrders();

            InitUserData();
        }

        private async void FillUserOrders()
        {
            var login = (string)App.Current.Properties["UserLogin"];
            var response = await _orderService.GetUserBasket(login);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                List<dgOrderVM> userProducts = new();
                foreach (var order in response.Data)
                {
                    userProducts.Add(new dgOrderVM()
                    {
                        IdOrder = (int)order.Id,
                        UserOfOrder = login,
                        UserPayMethod = order.PayMethod,
                        UserGetMethod = order.GetMethod,
                        DateOrder = order.DateCreate.ToString(),
                        AddressOrder = order.Address,
                        CountFood = order.Quantity,
                        AllPrice = order.Sum,
                        Commentary = order.Comment,
                    });

                }
                dgListOrdersReady.ItemsSource = userProducts; 
            }
            else { MessageBox.Show(response.Description); }
        }

        private async void InitUserData()
        {
            var login = (string)App.Current.Properties["UserLogin"];
            var response = await _userService.GetUsers();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var user = response.Data.FirstOrDefault(x => x.Login == login);
                if (user != null)
                {
                    FIO.Content += user.FIO;
                    Telephone.Content += user.Telephone;
                    Age.Content += user.Age.ToString();
                    var addresses = await _userService.GetAllAddresses(login);
                    if (addresses.StatusCode == Domain.Enum.StatusCode.OK)
                    {
                        foreach (var item in addresses.Data)
                        {
                            AddAdress.Content += $"\n{item.Street} {item.Home} {item.Apartment} {item.Entrance}";
                        }
                    }
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AuthPage(_userService, _db));
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.CartPage(_userService, _productService, _orderService));
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.MenuPage(_productService, _userService));
        }

        private void AddAdress_Click_1(object sender, RoutedEventArgs e)
        {
            var point = new AddAdress(_userService);
            point.ShowDialog();
        }
    }
}
