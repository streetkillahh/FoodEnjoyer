using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Response;
using FoodEnjoyerWPF.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Service.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<User>> Register(RegisterViewModel user);

        BaseResponse<Dictionary<int, string>> GetRoles();

        Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers();

        Task<IBaseResponse<bool>> DeleteUser(long id);
    }
}
