using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.SearchHelper;

namespace CarRent.Source.CarManagement.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();
        Task<Car> GetAsync(Guid id);
        Task AddAsync(Car obj);
        Task DeleteAsync(Car obj);
        Task UpdateAsync(Car obj);
        Task<List<Car>> Search(CarSearch search);
    }
}
