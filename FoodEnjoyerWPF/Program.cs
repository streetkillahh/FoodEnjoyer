using FoodEnjoyerWPF.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF
{
    public class Program
    {
        private static void Entry(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                // ...
            }
            else
            {
                // создаем хост приложения
                var host = Host.CreateDefaultBuilder()
                // внедряем сервисы
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.InitializeRepositories();
                    services.InitializeServices();
                })
                .Build();
                // получаем сервис - объект класса App
                var app = host.Services.GetService<App>();
                // запускаем приложения
                app?.InitializeComponent();
                app?.Run();
            }
        }
    }
}
