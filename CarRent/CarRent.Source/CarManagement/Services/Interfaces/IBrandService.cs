using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;

namespace CarRent.Source.CarManagement.Services.Interfaces
{
    public interface IBrandService
    {
        public Task<List<BrandDto>> GetAllAsync();
        public Task<BrandDto> GetByIdAsync(Guid id);
        public Task AddAsync(BrandDto obj);
        public Task UpdateAsync(BrandDto obj);
        public Task DeleteAsync(Guid id);
        public Task<List<BrandDto>> Search(string name);
    }
}
