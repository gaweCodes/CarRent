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
    public class CarCategoryRepository : ICarCategoryRepository
    {
        private readonly CarRentDbContext _dbContext;

        public CarCategoryRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CarCategory>> GetAllAsync() =>
            await _dbContext.CarCategories.AsQueryable().OrderBy(cc => cc.DailyFee).ToListAsync();

        public async Task<CarCategory> GetAsync(Guid id) =>
            await _dbContext.CarCategories.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(CarCategory obj)
        {
            await _dbContext.CarCategories.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CarCategory obj)
        {
            _dbContext.CarCategories.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarCategory obj)
        {
            _dbContext.CarCategories.Update(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<CarCategory>> Search(CarCategorySearch search)
        {
            var query = _dbContext.CarCategories.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(cc => cc.Name.Contains(search.Name, StringComparison.InvariantCultureIgnoreCase));
            if(search.Fee.HasValue)
                query = query.Where(cc => cc.DailyFee == search.Fee.Value);
            return await query.OrderBy(cc => cc.DailyFee).ToListAsync();
        }
}
}
