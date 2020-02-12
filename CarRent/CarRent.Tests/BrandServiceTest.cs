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
    public class BrandTests
    {
        [Test]
        public async Task BrandService_GetAllBrands_ListOfBrandsWithOneBrand()
        {
            // Arange
            var listToReturn = new List<Brand> {new Brand()};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDto>());
            var repo = A.Fake<IBrandRepository>();
            var brandservice = new BrandService(repo, config.CreateMapper());

            // Act
            A.CallTo(() => repo.GetAllAsync()).Returns(listToReturn);
            var result = await brandservice.GetAllAsync();

            // Assert
            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task BrandService_GetBrandById_GetExactlyOneBrandWithSpecifiedValues()
        {
            // Arange
            var objectToReturn = new Brand { Id = Guid.Empty, Title = "Test" };
            var expectedOjbect = new BrandDto { Id = Guid.Empty, Title = "Test" };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDto>());
            var repo = A.Fake<IBrandRepository>();
            var brandservice = new BrandService(repo, config.CreateMapper());

            // Act
            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(objectToReturn);
            var result = await brandservice.GetByIdAsync(Guid.Empty);

            // Assert
            Assert.That(result.Title, Is.EqualTo(expectedOjbect.Title));
            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
        }
    }
}