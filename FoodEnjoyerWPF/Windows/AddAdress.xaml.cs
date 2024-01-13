using FoodEnjoyerWPF.Domain.ViewModels.Address;
using FoodEnjoyerWPF.Service.Implementations;
using Microsoft.Extensions.Configuration;
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
using System.Windows.Shapes;

namespace FoodEnjoyerWPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAdress.xaml
    /// </summary>
    public partial class AddAdress : Window
    {
        private readonly UserService _userService;
        public AddAdress(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private async void NewAdress_Click(object sender, RoutedEventArgs e)
        {
            var login = (string)App.Current.Properties["UserLogin"];
            var address = new AddressViewModel()
            {
                Street = NewStreet.Text,
                Home = NewHouse.Text,
                Apartment = int.Parse(NewApartment.Text),
                Entrance = byte.Parse(NewEntrance.Text)
            };

            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(address);
            if (!Validator.TryValidateObject(address, context, results, true))
            {
                string excepts = "";
                foreach (var error in results)
                {
                    excepts += error.ErrorMessage + "\n";
                }
                MessageBox.Show(excepts);
                return;
            }
            var response = await _userService.SetNewAddress(login, address);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                MessageBox.Show($"Адрес \"{address.Street} {address.Home} {address.Apartment} {address.Entrance}\" был успешно добавлен для пользователя \"{login}\"");
            }
        }
    }
}
