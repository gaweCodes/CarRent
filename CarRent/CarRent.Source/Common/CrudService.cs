using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRent.Source.Common
{
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public CrudService(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<List<T>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<T> GetByIdAsync(Guid id) => await _repository.GetAsync(id);
        public async Task<T> AddBrandAsync(T obj) => await _repository.AddAsync(obj);
        public async Task UpdateAsync(T obj) => await _repository.UpdateAsync(obj);
        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
        public async Task<bool> CheckIfExistAsync(Guid id) => await _repository.ExistAsync(id);
    }
}
