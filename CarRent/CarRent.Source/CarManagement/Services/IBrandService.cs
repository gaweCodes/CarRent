using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;

namespace CarRent.Source.CarManagement.Services
{
    public interface IBrandService
    {
        public Task<List<Brand>> GetAllBrandsAsync();
        public Task<Brand> GetByIdAsync(Guid id);
        public Task<Brand> AddBrandAsync(Brand brand);
        public Task UpdateBrandAsync(Brand brand);
        public Task DeleteBrandAsync(Guid id);
        public Task<bool> CheckIfBrandExistAsync(Guid id);
    }
}
