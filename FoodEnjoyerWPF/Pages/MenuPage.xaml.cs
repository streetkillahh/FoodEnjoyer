using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Extensions;
using FoodEnjoyerWPF.Service.Implementations;
using FoodEnjoyerWPF.Windows;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace FoodEnjoyerWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;

        private string lastCLick = string.Empty;
        private DropShadowEffect LightShadow = new DropShadowEffect();
        private DropShadowEffect RedShadow = new DropShadowEffect();
        private DropShadowEffect DefaultShadow = new DropShadowEffect();
        public MenuPage(ProductService productService, UserService userService)
        {
            InitializeComponent();
            _productService = productService;
            _userService = userService;

            this.Loaded += (s, e) => FillPanel();

            LightShadow.BlurRadius = 30; LightShadow.Opacity = 0.6; LightShadow.Color = Colors.White; LightShadow.ShadowDepth = 0;
            RedShadow.Color = Colors.Red; RedShadow.Opacity = 1; RedShadow.ShadowDepth = 0; RedShadow.BlurRadius = 30;
            DefaultShadow = new()
            {
                Color = Color.FromRgb(250, 205, 176),
                ShadowDepth = 0,
                BlurRadius = 10
            };


        }

        private void FillPanel()
        {
            var response = _productService.GetProducts();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                ListOfTitles.Children.Clear();
                var BasketList = (List<string>)App.Current.Properties["Basket"] ?? new List<string>();
                foreach (var product in response.Data)
                {
                    var NItem = AddViewItem(product);    //подсветка уже выбраных ранее продуктов
                    NItem.Effect = BasketList.Contains(product.Name.Replace(" ", "_")) ? RedShadow : DefaultShadow;   
                    ListOfTitles.Children.Add(NItem);
                }
            }
            else { MessageBox.Show(response.Description); }
        }

        private Border AddViewItem(Product product)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 228, 196)),
                Margin = new Thickness(25),
                Padding = new Thickness(0, 0, 0, 20),
                Effect = DefaultShadow,
                Name = product.Name.Replace(" ", "_"),
            };

            border.MouseDown += (t, e) => border.Effect = border.Effect == RedShadow ? LightShadow : RedShadow;
            border.MouseEnter += (s, a) => border.Effect = border.Effect == RedShadow ? RedShadow : LightShadow;
            border.MouseLeave += (x, i) => border.Effect = border.Effect == RedShadow ? RedShadow : DefaultShadow;

            var stackPanel = new StackPanel();
            
            var tooltip = new ToolTip
            {
                Width = 300,
                Content = new TextBlock
                {
                    Text = product.Description,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                }
            };

            var img = new System.Windows.Controls.Image
            {
                Height = 150,
                Width = 150,
                Margin = new Thickness(10),
                Source = new BitmapImage(new Uri(product.ImgSource)),
                ToolTip = tooltip
            };

            var NameText = new TextBlock
            {
                Text = product.Name,
                Foreground = new SolidColorBrush(Colors.Black),
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Width = 160
            };

            var dockPanel = new DockPanel();
            var price = new TextBlock
            {
                Text = $"{product.Price}p",
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 12,
                Margin = new Thickness(10, 15, 10, 0),
                Foreground = new SolidColorBrush(Colors.Red),
                TextAlignment = TextAlignment.Left
            };
            DockPanel.SetDock(price, Dock.Left);

            var Category = new TextBlock
            {
                Text = product.Category.GetDisplayName(),
                HorizontalAlignment = HorizontalAlignment.Right,
                FontSize = 12,
                Margin = new Thickness(10, 15, 10, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(105, 105, 105)),
                TextAlignment = TextAlignment.Right
            };
            DockPanel.SetDock(Category, Dock.Right);

            dockPanel.Children.Add(price);
            dockPanel.Children.Add(Category);

            var btn = new Button
            {
                Margin = new Thickness(10, 10, 10, 0),
                Content = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(213, 113, 63)),
                    Text = "Добавить",
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                },
                Foreground = new SolidColorBrush(Color.FromRgb(251, 206, 177)), 
            };
            btn.Click += (t, e) => border.Effect = border.Effect == RedShadow ? LightShadow : RedShadow;

            stackPanel.Children.Add(img);
            stackPanel.Children.Add(NameText);
            stackPanel.Children.Add(dockPanel);
            stackPanel.Children.Add(btn);

            border.Child = stackPanel;

            return border;
            //ListOfTitles.Children.Add(border);

        }
        //addNewProd
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var addProd = new AddContent(_productService);
            addProd.ShowDialog();

            var NItem = (Product)App.Current.Properties["NewProduct"];
            var border = AddViewItem(NItem);

            //ListOfTitles.Children.Add(border);

        }
        //обновить
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
        //Check Items from Menu and be ready push them in basket
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            List<string> BasketList = new();
            foreach (UIElement item in ListOfTitles.Children) 
                if (item is Border && item.Effect == RedShadow) BasketList.Add(((Border)item).Name.Replace(" ", "_"));
            App.Current.Properties["Basket"] = BasketList;
            NavigationService.GoBack();
        }

        private void ButtonHots_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSoups_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSnacks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDesserts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDrinks_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
