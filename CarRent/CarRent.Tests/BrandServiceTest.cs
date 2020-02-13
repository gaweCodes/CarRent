using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using FakeItEasy;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.Services;
using NUnit.Framework;

namespace CarRent.Tests
{
    public class BrandServiceTest
    {
        [Test]
        public async Task BrandService_GetAllBrands_ListOfBrandsWithOneBrand()
        {
            var listToReturn = new List<Brand> {new Brand()};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDto>());
            var repo = A.Fake<IBrandRepository>();
            var brandservice = new BrandService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAllAsync()).Returns(listToReturn);
            var result = await brandservice.GetAllAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task BrandService_GetBrandById_GetExactlyOneBrandWithSpecifiedValues()
        {
            var objectToReturn = new Brand { Id = Guid.Empty, Title = "Test" };
            var expectedOjbect = new BrandDto { Id = Guid.Empty, Title = "Test" };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDto>());
            var repo = A.Fake<IBrandRepository>();
            var brandservice = new BrandService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(objectToReturn);
            var result = await brandservice.GetByIdAsync(Guid.Empty);

            Assert.That(result.Title, Is.EqualTo(expectedOjbect.Title));
            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
        }
    }
}