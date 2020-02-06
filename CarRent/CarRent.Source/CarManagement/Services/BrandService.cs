using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        public BrandService(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<List<Brand>> GetAllBrandsAsync() => await _brandRepository.GetAllAsync();
        public async Task<Brand> GetByIdAsync(Guid id) => await _brandRepository.GetAsync(id);

        public async Task<Brand> AddBrandAsync(Brand brand)
        { 
            return await _brandRepository.AddAsync(brand);
        }

        public async Task UpdateBrandAsync(Brand brand) => await _brandRepository.UpdateAsync(brand);
        public async Task DeleteBrandAsync(Guid id) => await _brandRepository.DeleteAsync(id);
        public async Task<bool> CheckIfBrandExistAsync(Guid id) => await _brandRepository.ExistAsync(id);
    }
}
