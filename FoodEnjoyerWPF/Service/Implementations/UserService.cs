using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.Extensions;
using FoodEnjoyerWPF.Domain.Helpers;
using FoodEnjoyerWPF.Domain.Response;
using FoodEnjoyerWPF.Domain.ViewModels.Address;
using FoodEnjoyerWPF.Domain.ViewModels.User;
using FoodEnjoyerWPF.FoodEnjoyer.DAL;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Interfaces;
using FoodEnjoyerWPF.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FoodEnjoyerWPF.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;

        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IBaseResponse<User>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null) 
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                return new BaseResponse<User>()
                {
                    Data = user,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $" GetUser({id}) {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Address>> SetNewAddress(string UserName, AddressViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Login == UserName);
                if (user == null)
                {
                    return new BaseResponse<Address>()
                    {
                        Description = $"[SetNewAddress]: Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                var address = new Address()
                {
                    Street = model.Street,
                    Home = model.Home,
                    Apartment = model.Apartment,
                    Entrance = byte.Parse(model.Entrance.ToString())
                };

                user.Addresses.Add(address);
                _userRepository.Update(user);
                
                return new BaseResponse<Address>()
                {
                    Data = address,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Address>()
                {
                    Description = $"[SetNewAddress]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<BaseResponse<List<Address>>> GetAllAddresses(string UserName)
        {
            try
            {
                var user = await _userRepository.GetAll().Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Login == UserName);
                if (user == null)
                {
                    return new BaseResponse<List<Address>>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                return new BaseResponse<List<Address>>()
                {
                    Data = user.Addresses,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Address>>()
                {
                    Description = $"[GetAllAddresses]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassowrd(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или логин"
                    };
                }
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Login]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            //Application
            return new ClaimsIdentity(claims, "Cookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        public async Task<IBaseResponse<User>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
                if (user != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                        StatusCode = StatusCode.UserAlreadyExists
                    };
                }
                user = new User();

                user.Login = model.Login;
                user.Role = (Role)model.Role;
                user.Password = HashPasswordHelper.HashPassowrd(model.Password);
                user.FIO = model.FIO;
                user.Age = model.Age;
                user.Telephone = model.Telephone;

                await _userRepository.Create(user);

                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = "Пользователь добавлен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[UserService.Register] error: {ex.Message}"
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetRoles()
        {
            try
            {
                var roles = ((Role[])Enum.GetValues(typeof(Role)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = roles,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAll().Include(x => x.Orders).Include(x => x.Addresses)
                    .Select(x => new UserViewModel()
                    {
                        Id = x.Id,
                        Login = x.Login,
                        FIO = x.FIO,
                        Role = x.Role.GetDisplayName(),
                        Password = HashPasswordHelper.HashPassowrd(x.Password),
                        Age = x.Age,
                        Telephone = x.Telephone,
                        Orders = x.Orders
                    })
                    .ToListAsync();

                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    Data = users,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[UserSerivce.GetUsers] error: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUser(long id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }
                await _userRepository.Delete(user);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[UserSerivce.DeleteUser] error: {ex.Message}"
                };
            }
        }
    }
}
