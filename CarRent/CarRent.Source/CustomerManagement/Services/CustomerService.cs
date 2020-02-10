using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.CustomerManagement.Dtos;
using CarRent.Source.CustomerManagement.Repositories.Interfaces;
using CarRent.Source.CustomerManagement.SearchHelper;
using CarRent.Source.CustomerManagement.Services.Interfaces;

namespace CarRent.Source.CustomerManagement.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customerList = await _repository.GetAllAsync();
            return _mapper.Map<List<Customer>, List<CustomerDto>>(customerList);
        }
        public async Task<CustomerDto> GetByIdAsync(Guid id) => _mapper.Map<Customer, CustomerDto>(await _repository.GetAsync(id));
        public async Task AddAsync(CustomerDto obj) => await _repository.AddAsync(_mapper.Map<CustomerDto, Customer>(obj));
        public async Task UpdateAsync(CustomerDto obj) => await _repository.UpdateAsync(_mapper.Map<CustomerDto, Customer>(obj));

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _repository.GetAsync(id);
            if (customer != default)
                await _repository.DeleteAsync(customer);
        }
        public async Task<List<CustomerDto>> Search(CustomerSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.FirstName) && string.IsNullOrWhiteSpace(search.LastName) && string.IsNullOrWhiteSpace(search.Address))
                return _mapper.Map<List<Customer>, List<CustomerDto>>(await _repository.GetAllAsync());
            return _mapper.Map<List<Customer>, List<CustomerDto>>(await _repository.Search(search));
        }
    }
}
