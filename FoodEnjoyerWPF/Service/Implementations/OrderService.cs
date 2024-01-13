using FoodEnjoyerWPF.DAL.Repositories;
using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.Extensions;
using FoodEnjoyerWPF.Domain.Response;
using FoodEnjoyerWPF.Domain.ViewModels.Order;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories;
using FoodEnjoyerWPF.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ProductRepository _productRepository;
        private readonly UserRepository _userRepository;
        private readonly OrderRepository _orderRepository;
        public OrderService(ProductRepository productRepository, UserRepository userRepository, OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IBaseResponse<Order>> Create(CreateOrderViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Orders)
                    .Include(x => x.Addresses)
                    .FirstOrDefaultAsync(x => x.Login == model.Login);
                if (user == null)
                {
                    return new BaseResponse<Order>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var order = new Order()
                {
                    DateCreate = DateTime.Now,
                    Quantity = model.Quantity,
                    Sum = model.Sum,
                    PayMethod = Enum.Parse<PayMethod>(model.PayMethod),
                    GetMethod = Enum.Parse<GetMethod>(model.GetMethod),
                    Status = false,
                    Comment = model.Comment,
                    UserId = user.Id,
                    AddressId = model.AddressId,
                    Products = model.Products
                };

                await _orderRepository.Create(order);

                return new BaseResponse<Order>()
                {
                    Description = "Заказ создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public Task<IBaseResponse<bool>> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IBaseResponse<List<Order>>> GetOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAll().ToListAsync();
                if (orders == null)
                {
                    return new BaseResponse<List<Order>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.OrderNotFound
                    };
                }
                return new BaseResponse<List<Order>>()
                {
                    Data = orders,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Order>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<OrderViewModel>>> GetUserBasket(string userName)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Orders)
                    .FirstOrDefaultAsync(x => x.Login == userName);

                if (user == null)
                {
                    return new BaseResponse<List<OrderViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var orders = user.Orders;
                var response = new List<OrderViewModel>();
                foreach (var order in orders)
                {
                    response.Add(new OrderViewModel()
                    {
                        DateCreate = order.DateCreate.ToString(),
                        Quantity = order.Quantity,
                        Sum = order.Sum,
                        PayMethod = order.PayMethod.GetDisplayName(),
                        GetMethod = order.GetMethod.GetDisplayName(),
                        Comment = order.Comment,
                        Address = $"ул.{order.Address.Street} дом: {order.Address.Home} кв.{order.Address.Apartment} подъезд - {order.Address.Entrance}",
                        Status = order.Status ? "Выполнен" : "Выполняется"
                    }) ;
                }
                
                return new BaseResponse<List<OrderViewModel>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<OrderViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
