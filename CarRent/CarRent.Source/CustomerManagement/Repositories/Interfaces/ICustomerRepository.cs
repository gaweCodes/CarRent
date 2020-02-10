using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.CustomerManagement.SearchHelper;

namespace CarRent.Source.CustomerManagement.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetAsync(Guid id);
        Task AddAsync(Customer obj);
        Task DeleteAsync(Customer obj);
        Task UpdateAsync(Customer obj);
        Task<List<Customer>> Search(CustomerSearch search);
    }
}
