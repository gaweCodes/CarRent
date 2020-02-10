using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.SearchHelper;

namespace CarRent.Source.CarManagement.Services.Interfaces
{
    public interface ICarCategoryService
    {
        public Task<List<CarCategoryDto>> GetAllAsync();
        public Task<CarCategoryDto> GetByIdAsync(Guid id);
        public Task AddAsync(CarCategoryDto obj);
        public Task UpdateAsync(CarCategoryDto obj);
        public Task DeleteAsync(Guid id);
        public Task<List<CarCategoryDto>> Search(CarCategorySearch search);
    }
}
