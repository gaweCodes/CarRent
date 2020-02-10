using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using CarRent.Source.CarManagement.SearchHelper;
using CarRent.Source.CarManagement.Services.Interfaces;

namespace CarRent.Source.CarManagement.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _repository;
        private readonly IMapper _mapper;
        public CarService(ICarRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CarDto>> GetAllAsync()
        {
            var carList = await _repository.GetAllAsync();
            return _mapper.Map<List<Car>, List<CarDto>>(carList);
        }
        public async Task<CarDto> GetByIdAsync(Guid id) => _mapper.Map<Car, CarDto>(await _repository.GetAsync(id));
        public async Task AddAsync(CarDto obj) => await _repository.AddAsync(_mapper.Map<CarDto, Car>(obj));
        public async Task UpdateAsync(CarDto obj) => await _repository.UpdateAsync(_mapper.Map<CarDto, Car>(obj));

        public async Task DeleteAsync(Guid id)
        {
            var car = await _repository.GetAsync(id);
            if (car != default)
                await _repository.DeleteAsync(car);
        }
        public async Task<List<CarDto>> Search(CarSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.CarNumber) && !search.CarModelid.HasValue)
                return _mapper.Map<List<Car>, List<CarDto>>(await _repository.GetAllAsync());
            return _mapper.Map<List<Car>, List<CarDto>>(await _repository.Search(search));
        }
    }
}
