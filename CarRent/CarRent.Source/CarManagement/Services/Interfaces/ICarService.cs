using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.SearchHelper;

namespace CarRent.Source.CarManagement.Services.Interfaces
{
    public interface ICarService
    {
        public Task<List<CarDto>> GetAllAsync();
        public Task<CarDto> GetByIdAsync(Guid id);
        public Task AddAsync(CarDto obj);
        public Task UpdateAsync(CarDto obj);
        public Task DeleteAsync(Guid id);
        public Task<List<CarDto>> Search(CarSearch search);
    }
}
