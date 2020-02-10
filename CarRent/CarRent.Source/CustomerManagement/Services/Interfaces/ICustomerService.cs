using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CustomerManagement.Dtos;
using CarRent.Source.CustomerManagement.SearchHelper;

namespace CarRent.Source.CustomerManagement.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<CustomerDto>> GetAllAsync();
        public Task<CustomerDto> GetByIdAsync(Guid id);
        public Task AddAsync(CustomerDto obj);
        public Task UpdateAsync(CustomerDto obj);
        public Task DeleteAsync(Guid id);
        public Task<List<CustomerDto>> Search(CustomerSearch search);
    }
}
