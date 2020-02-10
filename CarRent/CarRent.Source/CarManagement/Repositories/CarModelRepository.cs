using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using CarRent.Source.CarManagement.SearchHelper;
using CarRent.Source.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.CarManagement.Repositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly CarRentDbContext _dbContext;
        public CarModelRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CarModel>> GetAllAsync() =>
            await _dbContext.CarModels.AsQueryable().OrderBy(cm => cm.Title).ToListAsync();

        public async Task<CarModel> GetAsync(Guid id) =>
            await _dbContext.CarModels.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(CarModel obj)
        {
            await _dbContext.CarModels.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CarModel obj)
        {
            _dbContext.CarModels.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarModel obj)
        {
            _dbContext.CarModels.Update(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<CarModel>> Search(CarModelSearch search)
        {
            var query = _dbContext.CarModels.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(cm => cm.Title.Contains(search.Name, StringComparison.InvariantCultureIgnoreCase));
            if(search.BrandId.HasValue)
                query = query.Where(cm => cm.BrandId == search.BrandId.Value);
            if (search.CategoryId.HasValue)
                query = query.Where(cm => cm.CategoryId == search.CategoryId.Value);
            return await query.OrderBy(cm => cm.Title).ToListAsync();
        }
}
}
