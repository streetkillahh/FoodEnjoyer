using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.Extensions;
using FoodEnjoyerWPF.Domain.ViewModels.Product;
using FoodEnjoyerWPF.Service.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
using System.Windows.Shapes;
using System.Windows.Xps.Serialization;

namespace FoodEnjoyerWPF.Windows
{
    public partial class AddContent : Window
    {
        private readonly ProductService _productService;
        private string _title;

        //UPDATE
        public AddContent(ProductService productService, ProductViewModel product)
        {
            AddOptions();
            UpsertProduct.Content = "Изменить";

            _productService = productService;

            TitleName.Text = product.Name;
            comboBoxWithItems.Text = product.Category;
            Description.Text = product.Description;
            Price.Text = product.Price.ToString();

            ImgAddedTitle.Source = new BitmapImage(new Uri(product.ImgSource));



            UpsertProduct.Click += async (s, e) =>
             {
                var item = new ProductViewModel();

                item = CollectFieldsData(product);

                var response = await _productService.Edit(product.Id, item);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    MessageBox.Show($"продукт \"{item.Name}\" был успешно добавлен в бд");
                    App.Current.Properties["NewProduct"] = response.Data;
                    this.Close();
                }
                else { MessageBox.Show(response.Description); }

                Close();
            };
        }
        private void AddOptions()
        {
            InitializeComponent();
            Price.PreviewTextInput += (s, e) =>
            {
                if (!(char.IsDigit(e.Text, 0) || (e.Text == ".")
                    && (!Price.Text.Contains(".")
                    && Price.Text.Length != 0)))
                {
                    e.Handled = true;
                }
            };
        }

        //CREATE
        public AddContent(ProductService productService)
        {
            AddOptions();
            _productService = productService;

            UpsertProduct.Click += async (s, e) =>
            {
                var item = new ProductViewModel();

                item = CollectFieldsData(item);

                var response = await _productService.Create(item);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    MessageBox.Show($"продукт \"{item.Name}\" был успешно добавлен в бд");
                    App.Current.Properties["NewProduct"] = response.Data;
                    this.Close();
                }
                else { MessageBox.Show(response.Description); }

                Close();
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            if (ofd.ShowDialog() == true)
            {
                string ImgName = $"{ofd.FileName}";
                ImgAddedTitle.Source = new BitmapImage(new Uri(ofd.FileName));
            }
        }

        private ProductViewModel CollectFieldsData(ProductViewModel product)
        {
            product = new ProductViewModel()
            {
                Id=  product.Id,
                Name = TitleName.Text ?? " ",
                Category = comboBoxWithItems.Text == string.Empty ? null : comboBoxWithItems.Text,
                Description = Description.Text ?? "",
                Price = double.Parse(string.IsNullOrWhiteSpace(Price.Text) ? "0" : Price.Text.Replace(" ", "")),
                Count = product?.Count ?? 0,
                ImgSource = ImgAddedTitle.Source?.ToString() ?? null
            };

            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(product);
            var txtExcepts = "";

            if (!Validator.TryValidateObject(product, context, results, true))
            {
                foreach (var error in results)
                    txtExcepts += error.ErrorMessage + " \n";
                MessageBox.Show(txtExcepts);
                return null;
            }

            var imginfo = new FileInfo(product.ImgSource.Substring(8));
            string newPath = $"{Environment.CurrentDirectory}\\Images\\Products\\{product.Name}{imginfo.Extension}";
            //imginfo.MoveTo(newPath);
            if (!File.Exists(newPath)) System.IO.File.Copy(product.ImgSource.Substring(8), newPath);
            product.ImgSource = newPath;

            return product;
        }

        private async void AddTitleButton_Click(object sender, RoutedEventArgs e)
        {
            //var CB = (ComboBoxItem)comboBoxWithItems.SelectedItem;
            //Category = (Category)Convert.ToInt32(comboBoxWithItems.Text)
            //{
            //    Name = TitleName.Text,
            //    Category = comboBoxWithItems.Text == "Все категории" ? "" : comboBoxWithItems.Text,
            //    Description = Description.Text ?? "",
            //    Price = double.Parse(Price.Text?.Replace(" ", "") ?? "0"),
            //    Count = 0,
            //    ImgSource = ImgAddedTitle.Source.ToString() ?? null
            //};
        }
    }
}
