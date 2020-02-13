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
    public class CategoryServiceTest
    {
        [Test]
        public async Task CategoryService_GetAllCategories_ListOfCategoriesWithOneCategory()
        {
            var categoriesToReturn = new List<CarCategory> {new CarCategory{Id = Guid.Empty, DailyFee = 20m, Name = "Test" }};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CarCategory, CarCategoryDto>());
            var repo = A.Fake<ICarCategoryRepository>();
            var categoryService = new CarCategoryService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAllAsync()).Returns(categoriesToReturn);
            var result = await categoryService.GetAllAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task CategoryServiceService_GetCategoryById_GetExactlyOneCategoryWithSpecifiedValues()
        {
            var categoryToReturn = new CarCategory { Id = Guid.Empty, Name = "Test", DailyFee = 300m};
            var expectedOjbect = new CarCategoryDto { Id = Guid.Empty, Name = "Test", DailyFee = 300m };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CarCategory, CarCategoryDto>());
            var repo = A.Fake<ICarCategoryRepository>();
            var categoryService = new CarCategoryService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(categoryToReturn);
            var result = await categoryService.GetByIdAsync(Guid.Empty);

            Assert.That(result.DailyFee, Is.EqualTo(expectedOjbect.DailyFee));
            Assert.That(result.Name, Is.EqualTo(expectedOjbect.Name));
            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
        }
    }
}