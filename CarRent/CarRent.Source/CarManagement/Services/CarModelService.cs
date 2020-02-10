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
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _repository;
        private readonly IMapper _mapper;
        public CarModelService(ICarModelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CarModelDto>> GetAllAsync()
        {
            var carModelList = await _repository.GetAllAsync();
            return _mapper.Map<List<CarModel>, List<CarModelDto>>(carModelList);
        }
        public async Task<CarModelDto> GetByIdAsync(Guid id) => _mapper.Map<CarModel, CarModelDto>(await _repository.GetAsync(id));
        public async Task AddAsync(CarModelDto obj) => await _repository.AddAsync(_mapper.Map<CarModelDto, CarModel>(obj));
        public async Task UpdateAsync(CarModelDto obj) => await _repository.UpdateAsync(_mapper.Map<CarModelDto, CarModel>(obj));

        public async Task DeleteAsync(Guid id)
        {
            var carModel = await _repository.GetAsync(id);
            if (carModel != default)
                await _repository.DeleteAsync(carModel);
        }
        public async Task<List<CarModelDto>> Search(CarModelSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Name) && !search.BrandId.HasValue && !search.CategoryId.HasValue)
                return _mapper.Map<List<CarModel>, List<CarModelDto>>(await _repository.GetAllAsync());
            return _mapper.Map<List<CarModel>, List<CarModelDto>>(await _repository.Search(search));
        }
    }
}
