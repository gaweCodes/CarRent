using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.SearchHelper;

namespace CarRent.Source.CarManagement.Repositories.Interfaces
{
    public interface ICarModelRepository
    {
        Task<List<CarModel>> GetAllAsync();
        Task<CarModel> GetAsync(Guid id);
        Task AddAsync(CarModel obj);
        Task DeleteAsync(CarModel obj);
        Task UpdateAsync(CarModel obj);
        Task<List<CarModel>> Search(CarModelSearch search);
    }
}
