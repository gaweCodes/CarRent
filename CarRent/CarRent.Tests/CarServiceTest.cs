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
    public class CarServiceTest
    {
        [Test]
        public async Task CarService_GetAllCars_ListOfCarsWithOneCar()
        {
            var carsToReturn = new List<Car> {new Car{Id = Guid.Empty, CarNumber = "000001", CarModelid = Guid.Empty }};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDto>());
            var repo = A.Fake<ICarRepository>();
            var carService = new CarService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAllAsync()).Returns(carsToReturn);
            var result = await carService.GetAllAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task CarServiceService_GetCarById_GetExactlyOneCarWithSpecifiedValues()
        {
            var carToReturn = new Car { Id = Guid.Empty, CarNumber = "000001", CarModelid = Guid.Empty };
            var expectedOjbect = new CarDto { Id = Guid.Empty, CarNumber = "000001", CarModelid = Guid.Empty };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDto>());
            var repo = A.Fake<ICarRepository>();
            var carService = new CarService(repo, config.CreateMapper());

            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(carToReturn);
            var result = await carService.GetByIdAsync(Guid.Empty);

            Assert.That(result.CarNumber, Is.EqualTo(expectedOjbect.CarNumber));
            Assert.That(result.CarModelid, Is.EqualTo(expectedOjbect.CarModelid));
            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
        }
    }
}