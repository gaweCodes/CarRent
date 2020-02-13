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
    public class CarModelServiceTest
    {
        [Test]
        public async Task CarModelService_GetAllModels_ListOfModelsWithOneModel()
        {
            var modelsToReturn = new List<CarModel> {new CarModel {Id = Guid.Empty, Title = "Test", BrandId = Guid.Empty, CategoryId = Guid.Empty }};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CarModel, CarModelDto>());
            var repo = A.Fake<ICarModelRepository>();
            var modelService = new CarModelService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAllAsync()).Returns(modelsToReturn);
            var result = await modelService.GetAllAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task CategoryServiceService_GetCarModelById_GetExactlyOneCarModelWithSpecifiedValues()
        {
            var modelToReturn = new CarModel { Id = Guid.Empty, Title = "Test", BrandId = Guid.Empty, CategoryId = Guid.Empty };
            var expectedOjbect = new CarModelDto { Id = Guid.Empty, Title = "Test", BrandId = Guid.Empty, CategoryId = Guid.Empty };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CarModel, CarModelDto>());
            var repo = A.Fake<ICarModelRepository>();
            var modelService = new CarModelService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(modelToReturn);
            var result = await modelService.GetByIdAsync(Guid.Empty);

            Assert.That(result.CategoryId, Is.EqualTo(expectedOjbect.CategoryId));
            Assert.That(result.BrandId, Is.EqualTo(expectedOjbect.BrandId));
            Assert.That(result.Title, Is.EqualTo(expectedOjbect.Title));
            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
        }
    }
}