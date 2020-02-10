using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.CustomerManagement.Repositories.Interfaces;
using CarRent.Source.CustomerManagement.SearchHelper;
using CarRent.Source.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.CustomerManagement.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CarRentDbContext _dbContext;

        public CustomerRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Customer>> GetAllAsync() => await _dbContext.Customers.AsQueryable()
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();

        public async Task<Customer> GetAsync(Guid id) =>
            await _dbContext.Customers.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(Customer obj)
        {
            await _dbContext.Customers.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer obj)
        {
            _dbContext.Customers.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer obj)
        {
            _dbContext.Customers.Update(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Customer>> Search(CustomerSearch search)
        {
            var query = _dbContext.Customers.AsNoTracking().AsQueryable();
            if(!string.IsNullOrWhiteSpace(search.FirstName))
                query = query.Where(c => c.FirstName.Contains(search.FirstName, StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(search.LastName))
                query = query.Where(c => c.LastName.Contains(search.LastName, StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(search.Address))
                query = query.Where(c => c.Address.Contains(search.Address, StringComparison.InvariantCultureIgnoreCase));
            return await query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
        }
    }
}
