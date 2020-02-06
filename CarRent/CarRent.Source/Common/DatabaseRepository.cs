using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.Common
{
    public class DatabaseRepository<T> : IRepository<T> where T : class
    {
        private readonly CarRentDbContext _dbContext;
        public DatabaseRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<List<T>> GetAllAsync() => _dbContext.Set<T>().AsNoTracking().ToListAsync();
        public async Task<T> GetAsync(Guid id) => await _dbContext.FindAsync<T>(id);
        public async Task<T> AddAsync(T obj)
        {
            await _dbContext.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        public async Task DeleteAsync(Guid id)
        {
            var obj = await GetAsync(id);
            _dbContext.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(T obj)
        {
            _dbContext.Update(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> ExistAsync(Guid id)
        {
            var foundObject = await _dbContext.FindAsync<T>(id);
            return foundObject != null;
        }
    }
}
