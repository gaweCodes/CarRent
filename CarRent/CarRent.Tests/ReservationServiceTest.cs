using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.Services.Interfaces;
using CarRent.Source.ContractManagement.Dtos;
using CarRent.Source.ContractManagement.Services.Interfaces;
using FakeItEasy;
using CarRent.Source.ReservationManagement.Domain;
using CarRent.Source.ReservationManagement.Dtos;
using CarRent.Source.ReservationManagement.Repositories.Interfaces;
using CarRent.Source.ReservationManagement.Services;
using NUnit.Framework;

namespace CarRent.Tests
{
    public class ReservationServiceTest
    {
        [Test]
        public async Task ReservationService_GetAllReservations_ListOfReservationsWithOneReservation()
        {
            var reservationToReturn = new List<Reservation> {new Reservation()};
            var repo = A.Fake<IReservationRepository>();
            var carService = A.Fake<ICarService>();
            var carModelService = A.Fake<ICarModelService>();
            var carCategoryService = A.Fake<ICarCategoryService>();
            var rentalContractService = A.Fake<IRentalContractService>();

            var reservationservice = new ReservationService(repo, carService, carModelService, carCategoryService, rentalContractService);

            A.CallTo(() => repo.GetAllAsync()).Returns(reservationToReturn);
            var result = await reservationservice.GetAllAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }
        [Test]
        public async Task ReservationService_GetReservationById_GetExactlyOneReservationWithSpecifiedValues()
        {
            var objectToReturn = new Reservation { Id = Guid.Empty, DailyFee = 500m, CarId = Guid.Empty, CustomerId = Guid.Empty, TotalCost = 5000m, DurationInDays = 10, State = ReservationState.Active };
            var expectedOjbect = new ReservationDto { Id = Guid.Empty, State = "Active", CarId = Guid.Empty, CustomerId = Guid.Empty, DurationInDays = 10, TotalCost = 5000 };
            var repo = A.Fake<IReservationRepository>();
            var carService = A.Fake<ICarService>();
            var carModelService = A.Fake<ICarModelService>();
            var carCategoryService = A.Fake<ICarCategoryService>();
            var rentalContractService = A.Fake<IRentalContractService>();
            var reservationservice = new ReservationService(repo, carService, carModelService, carCategoryService, rentalContractService);

            A.CallTo(() => repo.GetAsync(Guid.Empty)).Returns(objectToReturn);
            var result = await reservationservice.GetByIdAsync(Guid.Empty);

            Assert.That(result.Id, Is.EqualTo(expectedOjbect.Id));
            Assert.That(result.CarId, Is.EqualTo(expectedOjbect.CarId));
            Assert.That(result.CustomerId, Is.EqualTo(expectedOjbect.CustomerId));
            Assert.That(result.DurationInDays, Is.EqualTo(expectedOjbect.DurationInDays));
            Assert.That(result.TotalCost, Is.EqualTo(expectedOjbect.TotalCost));
            Assert.That(result.State, Is.EqualTo(expectedOjbect.State));
        }
    }
}