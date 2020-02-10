using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;

namespace CarRent.Source.CarManagement.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task<Brand> GetAsync(Guid id);
        Task AddAsync(Brand obj);
        Task DeleteAsync(Brand obj);
        Task UpdateAsync(Brand obj);
        Task<List<Brand>> Search(string name);
    }
}
