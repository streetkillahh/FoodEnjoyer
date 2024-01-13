using FoodEnjoyerWPF.Domain.ViewModels.User;
using FoodEnjoyerWPF.FoodEnjoyer.DAL;
using FoodEnjoyerWPF.Service.Implementations;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
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
    public partial class AuthPage : Page
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _db;
        public AuthPage(UserService userService, ApplicationDbContext db)
        {
            InitializeComponent();
            _userService = userService;
            _db = db;
        }

        private async void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            var user = new LoginViewModel
            {
               Login = Login.Text ?? "",
               Password = PassBox.Password ?? ""
               //Login = "Admin",
               //Password = "123456"
            };
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(user);

            string txtExcepts = "";
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                foreach (var error in results)
                    txtExcepts += error.ErrorMessage + " \n";
                MessageBox.Show(txtExcepts);
                return;
            }
            var response = await _userService.Login(user);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var role = response.Data.Claims.ToList();
                MessageBox.Show("Пользователь успешно авторизовался");
                App.Current.Properties["UserLogin"] = user.Login;
                NavigationService.Navigate(role[1].Value == "Admin" ? new Pages.AdminProductsPage(_userService, _db) 
                    : new Pages.MainPage(_userService, _db));
            }
            else MessageBox.Show(response.Description);
        }

        private void Button_Page_Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.EnterPage(_userService, _db));
        }
    }
}
