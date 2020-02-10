using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.SearchHelper;

namespace CarRent.Source.CarManagement.Services.Interfaces
{
    public interface ICarModelService
    {
        public Task<List<CarModelDto>> GetAllAsync();
        public Task<CarModelDto> GetByIdAsync(Guid id);
        public Task AddAsync(CarModelDto obj);
        public Task UpdateAsync(CarModelDto obj);
        public Task DeleteAsync(Guid id);
        public Task<List<CarModelDto>> Search(CarModelSearch search);
    }
}
