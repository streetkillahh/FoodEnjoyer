using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Response;
using FoodEnjoyerWPF.Domain.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Service.Interfaces
{
    public interface IProductService
    {
        BaseResponse<Dictionary<int, string>> GetTypes();

        IBaseResponse<List<Product>> GetProducts();

        Task<IBaseResponse<ProductViewModel>> GetProduct(int id);


        Task<IBaseResponse<Product>> Create(ProductViewModel car);

        Task<IBaseResponse<bool>> DeleteProduct(long id);

        Task<IBaseResponse<Product>> Edit(long id, ProductViewModel model);
    }
}
