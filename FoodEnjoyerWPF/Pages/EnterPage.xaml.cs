using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.Extensions;
using FoodEnjoyerWPF.Domain.ViewModels.User;
using FoodEnjoyerWPF.FoodEnjoyer.DAL;
using FoodEnjoyerWPF.Service.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace FoodEnjoyerWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnterPage.xaml
    /// </summary>
    public partial class EnterPage : Page
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _db;
        public EnterPage(UserService userService, ApplicationDbContext db)
        {
            InitializeComponent();
            _userService = userService;
            _db = db;
        }

        private void Button_Page_Auth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AuthPage(_userService, _db));
        }
        //регистрация
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = new RegisterViewModel();
            user.Login = Login.Text ?? "";                      //собираем модель
            user.Password = PassBox.Password ?? "";             //с атрибутами
            user.PasswordConfirm = PassBox_2.Password ?? "";    //аннотации
            user.FIO = FIO.Text ?? "";                          //данных
            user.Age = byte.Parse(string.IsNullOrWhiteSpace(Age.Text) ? "0" : Age.Text);
            user.Telephone = Telephone.Text ?? "";
            user.Role = 0;
            
            //необх obj's для валидации
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(user);
            string txtExcept = "";
            //ищем ошибки
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                foreach (var error in results) //если есть выводим в msgBox
                {
                    txtExcept += error.ErrorMessage + "\n";
                }
                MessageBox.Show(txtExcept);
                return;
                
            }

            var response = await _userService.Register(user);
            if (response.StatusCode == StatusCode.OK) 
            {
                MessageBox.Show("Регистрация пользователя прошла успешно");
                NavigationService.Navigate(new Pages.MainPage(_userService, _db));
            }
            else MessageBox.Show(response.StatusCode.ToString() + ": " + response.Description);
        }
    }
}
