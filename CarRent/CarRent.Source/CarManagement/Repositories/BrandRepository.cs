using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using CarRent.Source.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.CarManagement.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly CarRentDbContext _dbContext;
        public BrandRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Brand>> GetAllAsync() => await _dbContext.Brands.AsQueryable().OrderBy(a => a.Title).ToListAsync();

        public async Task<Brand> GetAsync(Guid id) =>
            await _dbContext.Brands.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);
        public async Task AddAsync(Brand obj)
        {
            await _dbContext.Brands.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Brand obj)
        {
            _dbContext.Brands.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Brand obj)
        {
            _dbContext.Brands.Update(obj);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Brand>> Search(string name) => await _dbContext.Brands.AsNoTracking().AsQueryable()
            .Where(b => b.Title.Contains(name, StringComparison.InvariantCultureIgnoreCase)).OrderBy(b => b.Title).ToListAsync();
    }
}
