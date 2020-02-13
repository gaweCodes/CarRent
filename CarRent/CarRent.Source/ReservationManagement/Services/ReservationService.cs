using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Services.Interfaces;
using CarRent.Source.ContractManagement.Dtos;
using CarRent.Source.ContractManagement.Services.Interfaces;
using CarRent.Source.ReservationManagement.Domain;
using CarRent.Source.ReservationManagement.Dtos;
using CarRent.Source.ReservationManagement.Repositories.Interfaces;
using CarRent.Source.ReservationManagement.SearchHelper;
using CarRent.Source.ReservationManagement.Services.Interfaces;

namespace CarRent.Source.ReservationManagement.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly ICarService _carService;
        private readonly ICarModelService _carModelService;
        private readonly ICarCategoryService _carCategoryService;
        private readonly IRentalContractService _rentalContractService;
        public ReservationService(IReservationRepository repository, ICarService carService, ICarModelService carModelService, ICarCategoryService carCategoryService, IRentalContractService rentalContractService)
        {
            _repository = repository;
            _carService = carService;
            _carModelService = carModelService;
            _carCategoryService = carCategoryService;
            _rentalContractService = rentalContractService;
        }
        public async Task<List<ReservationDto>> GetAllAsync()
        {
            var reservationList = await _repository.GetAllAsync();
            var reservationListDtos = new List<ReservationDto>();
            reservationList.ForEach(r => { reservationListDtos.Add(Reservation.ToDto(r)); });
            return reservationListDtos;
        }
        public async Task<ReservationDto> GetByIdAsync(Guid id) => Reservation.ToDto(await _repository.GetAsync(id));

        public async Task AddAsync(ReservationDto obj)
        {
            var reservation = Reservation.FromDto(obj);
            var car = await _carService.GetByIdAsync(reservation.CarId);
            var carModel = await _carModelService.GetByIdAsync(car.CarModelid);
            var carCategory = await _carCategoryService.GetByIdAsync(carModel.CategoryId);
            reservation.DailyFee = carCategory.DailyFee;
            reservation.SetActive();
            reservation.CalculateTotal();
            await _repository.AddAsync(reservation);
        }

        public async Task UpdateAsync(ReservationDto obj)
        {
            if (obj.State == ReservationState.Closed.ToString())
                throw new InvalidOperationException("You can not update a contracted reservation");
            var reservation = Reservation.FromDto(obj);
            var car = await _carService.GetByIdAsync(reservation.CarId);
            var carModel = await _carModelService.GetByIdAsync(car.CarModelid);
            var carCategory = await _carCategoryService.GetByIdAsync(carModel.CategoryId);
            reservation.DailyFee = carCategory.DailyFee;
            reservation.SetActive();
            reservation.CalculateTotal();
            await _repository.UpdateAsync(reservation);
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await _repository.GetAsync(id);
            if (reservation != default)
                await _repository.DeleteAsync(reservation);
        }
        public async Task<List<ReservationDto>> Search(ReservationSearch search)
        {
            List<Reservation> reservationList;
            if (!search.State.HasValue && !search.CarId.HasValue && !search.CustomerId.HasValue &&
                !search.TotalCost.HasValue && !search.DurationInDays.HasValue)
                reservationList = await _repository.GetAllAsync();
            else
                reservationList = await _repository.Search(search);
            var reservationListDtos = new List<ReservationDto>();
            reservationList.ForEach(r => { reservationListDtos.Add(Reservation.ToDto(r)); });
            return reservationListDtos;
        }

        public async Task PickUp(ReservationDto obj)
        {
            var reservation = Reservation.FromDto(obj);
            var car = await _carService.GetByIdAsync(reservation.CarId);
            var carModel = await _carModelService.GetByIdAsync(car.CarModelid);
            var carCategory = await _carCategoryService.GetByIdAsync(carModel.CategoryId);
            reservation.DailyFee = carCategory.DailyFee;
            reservation.CalculateTotal();
            reservation.SetClosed();
            await _repository.UpdateAsync(reservation);
            var rentalContractDto = new RentalContractDto
            {
                Id = Guid.NewGuid(),
                CarId = reservation.CarId,
                CustomerId = reservation.CustomerId,
                DurationInDays = reservation.DurationInDays,
                ReservationId = reservation.Id,
                TotalCost = reservation.TotalCost
            };
            await _rentalContractService.CreateRentalContract(rentalContractDto);
        }
    }
}
