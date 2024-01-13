using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Response;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories;
using FoodEnjoyerWPF.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.Service.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly AddressRepository _addressRepository;
        public AddressService(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IBaseResponse<string>> GetAddress(int id)
        {
            try
            {
                var address = await _addressRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (address == null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "Адрес не найден",
                        StatusCode = Domain.Enum.StatusCode.OrderNotFound
                    };
                }
                return new BaseResponse<string>()
                {
                    Data = $"ул.{address.Street} дом: {address.Home} кв.{address.Apartment} подъезд - {address.Entrance}",
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $" GetAddress({id}) {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }
    }
}
