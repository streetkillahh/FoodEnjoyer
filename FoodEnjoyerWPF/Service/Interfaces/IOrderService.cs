using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Response;
using FoodEnjoyerWPF.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IBaseResponse<Order>> Create(CreateOrderViewModel model);
        Task<IBaseResponse<List<Order>>> GetOrders();
        Task<IBaseResponse<List<OrderViewModel>>> GetUserBasket(string userName); //все заказы конкретного юзера
        Task<IBaseResponse<bool>> Delete(long id);
        
        //Task<IBaseResponse<OrderViewModel>> GetItem(string userName, long id);  //у юзера конкретный заказ
    }
}
