using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.SearchHelper;

namespace CarRent.Source.CarManagement.Repositories.Interfaces
{
    public interface ICarCategoryRepository
    {
        Task<List<CarCategory>> GetAllAsync();
        Task<CarCategory> GetAsync(Guid id);
        Task AddAsync(CarCategory obj);
        Task DeleteAsync(CarCategory obj);
        Task UpdateAsync(CarCategory obj);
        Task<List<CarCategory>> Search(CarCategorySearch search);
    }
}
