using FoodEnjoyerWPF.DAL.Repositories;
using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Interfaces;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories;
using FoodEnjoyerWPF.Service.Implementations;
using FoodEnjoyerWPF.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Domain.Extensions
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<Product>, ProductRepository>();
            services.AddTransient<IBaseRepository<User>, UserRepository>();
            services.AddTransient<IBaseRepository<Address>, AddressRepository>();
            services.AddTransient<IBaseRepository<Order>, OrderRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
        }
    }
}
