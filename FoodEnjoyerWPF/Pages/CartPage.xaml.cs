using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.ViewModels.Order;
using FoodEnjoyerWPF.Service.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoodEnjoyerWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private List<Product> _products = new List<Product>();
        public CartPage(UserService userService, ProductService productService, OrderService orderService)
        {
            InitializeComponent();
            _userService = userService;
            _productService = productService;
            _orderService = orderService;

            FillPanel();
        }

        private async void PushAddressesInPanel()
        {
            var login = (string)App.Current.Properties["UserLogin"];
            var response = await _userService.GetAllAddresses(login);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                foreach (var item in response.Data)
                {
                    string address = $"ул.{item.Street} дом {item.Home} кв.{item.Apartment} подъезд {item.Entrance}";
                    var CBItem = new ComboBoxItem() { Content = address, Name = $"_{item.Id}" };
                    SetAddress.Items.Add(CBItem);
                }
            }
            else { MessageBox.Show(response.Description); }
        }

        private void FillPanel()
        {
            PushAddressesInPanel();

            var response = _productService.GetProducts();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                ListOfTitles.Children.Clear();
                var BasketList = (List<string>)App.Current.Properties["Basket"] ?? new List<string>();
                foreach (var item in BasketList)
                {
                    var product = new Product();
                    product = response.Data.First(x => x.Name == item.Replace("_", " "));
                    if (product is not null)
                    {
                        _products.Add(product);
                        var border = AddViewItem(product);
                        ListOfTitles.Children.Add(border);
                    }
                }
            }
            else { MessageBox.Show(response.Description); }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeletePosition_Click(object sender, RoutedEventArgs e)
        {

        }

        private Border AddViewItem(Product product)
        {
            var border = new Border()
            {
                Margin = new Thickness(10),
                Background = new SolidColorBrush(Color.FromRgb(251, 206, 177)),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
                Height = 150,
                VerticalAlignment = VerticalAlignment.Top,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 10,
                    Color = Colors.Black,
                    ShadowDepth = 2
                }
            };

            var grid = new Grid();

            var imgDelete = new Image() { Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\Images\\src\\Bin.png")) };
            var DeleteButton = new Button()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Content = imgDelete
            };

            var ProductImage = new Image()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 10, 0, 0),
                Width = 140,
                Height = 100,
                Source = new BitmapImage(new Uri(product.ImgSource))
            };

            grid.Children.Add(DeleteButton);
            grid.Children.Add(ProductImage);


            var dataBorder = new Border()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10),
                Width = 140,
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1)
            };
            var stck = new StackPanel() { Orientation = Orientation.Horizontal };
            var CountString = new TextBlock()
            {
                Text = "Количество ",
                Margin = new Thickness(5, 0, 5, 0),
                FontFamily = new FontFamily("TrebuchetMS"),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var CountValue = new ComboBox()
            {
                FontFamily = new FontFamily("TrebuchetMS"),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var SumAndQuantity = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(80, 0, 80, 0),
                FontSize = 13,
                FontFamily = new FontFamily("TrebuchetMS"),
                Text = string.IsNullOrWhiteSpace(CountValue.Text) ? $"{product.Price}p/шт" : $"{product.Price * int.Parse(CountValue.Text)}p/{CountValue.Text}шт"
            };

            for (int i = 1; i <= 5; i++)
            {
                var CBItem = new ComboBoxItem() { Content = i.ToString() };
                CBItem.Selected += (s, a) =>
                {
                    var item = s as ComboBoxItem;
                    int count = int.Parse(item.Content.ToString());
                    SumAndQuantity.Text = $"{product.Price * count}p/{count}шт";
                };
                CountValue.Items.Add(CBItem);
            }

            stck.Children.Add(CountString);
            stck.Children.Add(CountValue);
            dataBorder.Child = stck;

            grid.Children.Add(dataBorder);

            var pnl = new StackPanel();
            var nameProduct = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = product.Name,
                Margin = new Thickness(170, 10, 170, 10),
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("TrebuchetMS")
            };
            var Description = new TextBlock()
            {
                Width = 200,
                Height = 90,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = product.Description,
                Margin = new Thickness(170, 0, 170, 0),
                FontSize = 13,
                FontFamily = new FontFamily("TrebuchetMS")
            };

            pnl.Children.Add(nameProduct);
            pnl.Children.Add(Description);



            grid.Children.Add(pnl);
            grid.Children.Add(SumAndQuantity);

            border.Child = grid;
            return border;
        }


        private async void ButtonConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int count = 0;
            foreach (var item in _products)
            {
                item.Count++;
                sum += int.Parse(item.Price.ToString());
                count++;
            }
            SumOrder.Text += sum;
            if (string.IsNullOrEmpty(PaymentOption.Text)) MessageBox.Show("Выберите способ оплаты");
            else if (string.IsNullOrEmpty(DeliveryOption.Text)) MessageBox.Show("Выберите способ доставки");
            else if (string.IsNullOrEmpty(SetAddress.Text)) MessageBox.Show("Выберите адрес доставки");
            else
            {
                var cboItem = (ComboBoxItem)SetAddress.SelectedItem;
                int address_code = int.Parse(cboItem.Name.Replace("_", ""));
                var finallyOrder = new CreateOrderViewModel()
                {
                    DateCreated = DateTime.Now,
                    Quantity = count,
                    Sum = sum,
                    PayMethod = PaymentOption.Text,
                    GetMethod = DeliveryOption.Text,
                    AddressId = address_code,
                    Status = false,
                    Comment = Comment.Text,
                    Login = (string)App.Current.Properties["UserLogin"],
                    Products = _products
                };

                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var context = new ValidationContext(finallyOrder);
                var excepts = "";
                if (!Validator.TryValidateObject(finallyOrder, context, results, true))
                {
                    foreach (var error in results)
                    {
                        excepts += "\n" + error.ErrorMessage;
                    }
                    MessageBox.Show(excepts);
                    return;
                }

                var response = await _orderService.Create(finallyOrder);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    MessageBox.Show("Заказ был успешно добавлен");
                    ListOfTitles.Children.Clear();
                    App.Current.Properties["Basket"] = null;
                }
                else { MessageBox.Show(response.Description); }
            }
        }
    }
}
