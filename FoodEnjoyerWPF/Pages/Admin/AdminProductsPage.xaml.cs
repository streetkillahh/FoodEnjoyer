using FoodEnjoyerWPF.DAL.Repositories;
using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.Extensions;
using FoodEnjoyerWPF.Domain.ViewModels;
using FoodEnjoyerWPF.FoodEnjoyer.DAL;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories;
using FoodEnjoyerWPF.Service.Implementations;
using FoodEnjoyerWPF.Service.Interfaces;
using FoodEnjoyerWPF.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace FoodEnjoyerWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminProductsPage.xaml
    /// </summary>
    public partial class AdminProductsPage : Page
    {

        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly AddressService _addressService;
        private readonly ApplicationDbContext _db;
        public AdminProductsPage(UserService userService, ApplicationDbContext db)
        {
            InitializeComponent();
            _userService = userService;
            _db = db;
            _productService = new ProductService(new ProductRepository(_db));
            _orderService = new OrderService(new ProductRepository(_db) , new UserRepository(_db) ,new OrderRepository(_db));
            _addressService = new AddressService(new AddressRepository(_db));

            this.Loaded += (s, e) => FillProductsData();
            this.Loaded += (s, e) => FillUsersData();
            this.Loaded += (s, e) => FillOrdersData();


        }

        private async void FillOrdersData()
        {
            var response = await _orderService.GetOrders();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                List<dgOrderVM> TotalOrders = new();
                foreach (var order in response.Data)
                {
                    var user = await _userService.GetUser(order.UserId);
                    var address = await _addressService.GetAddress(order.AddressId);
                    TotalOrders.Add(new dgOrderVM()
                    {
                        IdOrder = (int)order.Id,
                        UserOfOrder = user.Data.Login,
                        UserPayMethod = order.PayMethod.GetDisplayName(),
                        UserGetMethod = order.GetMethod.GetDisplayName(),
                        DateOrder = order.DateCreate.ToString(),
                        AddressOrder = address.Data,
                        CountFood = order.Quantity,
                        AllPrice = order.Sum,
                        Commentary = order.Comment,
                    });

                }
                dgListOrdersReady.ItemsSource = TotalOrders;
            }
            else { MessageBox.Show(response.Description); }
        }

        private async void FillUsersData()
        {
            var response = await _userService.GetUsers();
            List<dgUserVM> usersData = new();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                foreach (var user in response.Data)
                {
                    int quantity = 0;
                    double totalSum = 0;
                    var orders = user.Orders;
                    foreach (var order in orders) 
                    {
                        quantity++;
                        totalSum += order.Sum;
                    }
                    var userVM = new dgUserVM()
                    {
                        IdUser = (int)user.Id,
                        Role = user.Role,
                        UserLogin = user.Login,
                        UserTelephone = user.Telephone,
                        UserFIO = user.FIO,
                        UserAge = user.Age,
                        TotalCost = totalSum,
                        TotalOrders = quantity,

                    };
                    usersData.Add(userVM);
                }
            }
            else { MessageBox.Show("Пользователей в бд нет"); }
            dgListUsers.ItemsSource = usersData;
        }

        private void FillProductsData()
        {
            var response = _productService.GetProducts();
            List<dgProductVM> prods = new();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                foreach (var item in response.Data)
                {
                    prods.Add(new dgProductVM
                    {
                        Id =            item.Id,
                        Name =          item.Name,
                        Category =      item.Category.GetDisplayName(),
                        Description =   item.Description,
                        Price =         item.Price,
                        Count =         item.Count,
                        Image = new Uri($"file:///{item.ImgSource}")

                    });
                }
                dgListOrders.ItemsSource = prods;
            }
            else
                MessageBox.Show("Список товаров пуст");
        }
        //AddNewProduct
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addProd = new AddContent(_productService);
            addProd.ShowDialog();

            //var NItem = (Product)App.Current.Properties["NewProduct"];
            FillProductsData();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var path = dgListOrders.SelectedItem as dgProductVM;
            if (path != null) 
            {
                var response = await _productService.DeleteProduct(path.Id);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    MessageBox.Show($"Товар \"{path.Name}\" был успешно удалён");
                    FillProductsData();
                }
                else MessageBox.Show(response.Description);
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var path = dgListOrders.SelectedItem as dgProductVM;
            if (path != null)
            {
                var response = await _productService.GetProduct(path.Id);
                if (response.StatusCode != Domain.Enum.StatusCode.OK) { MessageBox.Show(response.Description ); }

                var addProd = new AddContent(_productService, response.Data);
                addProd.ShowDialog();

                FillProductsData();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AuthPage(_userService, _db));
        }
    }
}
