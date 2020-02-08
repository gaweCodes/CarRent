using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRent.Source.Common
{
    public interface ICrudService<T>
    {
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(Guid id);
        public Task<T> AddBrandAsync(T obj);
        public Task UpdateAsync(T obj);
        public Task DeleteAsync(Guid id);
        public Task<bool> CheckIfExistAsync(Guid id);
    }
}
