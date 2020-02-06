using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRent.Source.Common
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> AddAsync(T obj);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T obj);
        Task<bool> ExistAsync(Guid id);
    }
}
