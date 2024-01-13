using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.FoodEnjoyer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodEnjoyerWPF.FoodEnjoyer.DAL.Repositories
{
    public class AddressRepository : IBaseRepository<Address>
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Address entity)
        {
            await _dbContext.Addresses.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Address> GetAll()
        {
            return _dbContext.Addresses;
        }

        public async Task Delete(Address entity)
        {
            _dbContext.Addresses.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Address> Update(Address entity)
        {
            _dbContext.Addresses.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
