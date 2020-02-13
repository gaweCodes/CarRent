using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.CustomerManagement.Dtos;
using CarRent.Source.CustomerManagement.Repositories.Interfaces;
using CarRent.Source.CustomerManagement.Services;
using NUnit.Framework;

namespace CarRent.Tests
{
    public class CustomerServiceTest
    {
        [Test]
        public async Task CustomerService_GetAllCustomers_ListOfCustomersWithOneCustomer()
        {
            var customerToReturn = new List<Customer> {new Customer()};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerDto>());
            var repo = A.Fake<ICustomerRepository>();
            var customerservice = new CustomerService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAllAsync()).Returns(customerToReturn);
            var result = await customerservice.GetAllAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task CustomerService_GetCustomerById_GetExactlyOneCustomerWithSpecifiedValues()
        {
            var objectToReturn = new Customer { Id = Guid.Empty, LastName = "Weibel", FirstName = "Gabriel", Address = $"Säntisstrasse 2{Environment.NewLine}9536 Schwarzenbach" };
            var expectedOjbect = new CustomerDto { Id = Guid.Empty, LastName = "Weibel", FirstName = "Gabriel", Address = $"Säntisstrasse 2{Environment.NewLine}9536 Schwarzenbach" };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerDto>());
            var repo = A.Fake<ICustomerRepository>();
            var customerservice = new CustomerService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(objectToReturn);
            var result = await customerservice.GetByIdAsync(Guid.Empty);

            Assert.That(result.FirstName, Is.EqualTo(expectedOjbect.FirstName));
            Assert.That(result.LastName, Is.EqualTo(expectedOjbect.LastName));
            Assert.That(result.Address, Is.EqualTo(expectedOjbect.Address));
            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
        }
    }
}