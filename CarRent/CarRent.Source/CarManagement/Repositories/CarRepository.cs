using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using CarRent.Source.CarManagement.SearchHelper;
using CarRent.Source.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.CarManagement.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CarRepository : ICarRepository
    {
        private readonly CarRentDbContext _dbContext;

        public CarRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Car>> GetAllAsync() =>
            await _dbContext.Cars.AsQueryable().OrderBy(c => c.CarNumber).ToListAsync();

        public async Task<Car> GetAsync(Guid id) =>
            await _dbContext.Cars.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(Car obj)
        {
            await _dbContext.Cars.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Car obj)
        {
            _dbContext.Cars.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car obj)
        {
            _dbContext.Cars.Update(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Car>> Search(CarSearch search)
        {
            var query = _dbContext.Cars.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.CarNumber))
                query = query.Where(c => c.CarNumber.Contains(search.CarNumber, StringComparison.InvariantCultureIgnoreCase));
            if(search.CarModelid.HasValue)
                query = query.Where(c => c.CarModelid == search.CarModelid.Value);
            return await query.OrderBy(c => c.CarNumber).ToListAsync();
        }
}
}
