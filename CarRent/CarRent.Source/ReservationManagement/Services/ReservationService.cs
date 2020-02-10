using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.CarManagement.Services.Interfaces;
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
        private readonly IMapper _mapper;
        public ReservationService(IReservationRepository repository, ICarService carService, ICarModelService carModelService, ICarCategoryService carCategoryService, IMapper mapper)
        {
            _repository = repository;
            _carService = carService;
            _carModelService = carModelService;
            _carCategoryService = carCategoryService;
            _mapper = mapper;
        }
        public async Task<List<ReservationDto>> GetAllAsync()
        {
            var reservationList = await _repository.GetAllAsync();
            return _mapper.Map<List<Reservation>, List<ReservationDto>>(reservationList);
        }
        public async Task<ReservationDto> GetByIdAsync(Guid id) => _mapper.Map<Reservation, ReservationDto>(await _repository.GetAsync(id));

        public async Task AddAsync(ReservationDto obj)
        {
            var reservation = _mapper.Map<ReservationDto, Reservation>(obj);
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
            var reservation = _mapper.Map<ReservationDto, Reservation>(obj);
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
            if (string.IsNullOrWhiteSpace(search.State) && !search.CarId.HasValue && !search.CustomerId.HasValue && !search.TotalCost.HasValue)
                return _mapper.Map<List<Reservation>, List<ReservationDto>>(await _repository.GetAllAsync());
            return _mapper.Map<List<Reservation>, List<ReservationDto>>(await _repository.Search(search));
        }
    }
}
