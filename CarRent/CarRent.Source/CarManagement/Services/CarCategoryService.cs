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
    public class CarCategoryService : ICarCategoryService
    {
        private readonly ICarCategoryRepository _repository;
        private readonly IMapper _mapper;
        public CarCategoryService(ICarCategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CarCategoryDto>> GetAllAsync()
        {
            var carCategoryList = await _repository.GetAllAsync();
            return _mapper.Map<List<CarCategory>, List<CarCategoryDto>>(carCategoryList);
        }
        public async Task<CarCategoryDto> GetByIdAsync(Guid id) => _mapper.Map<CarCategory, CarCategoryDto>(await _repository.GetAsync(id));
        public async Task AddAsync(CarCategoryDto obj) => await _repository.AddAsync(_mapper.Map<CarCategoryDto, CarCategory>(obj));
        public async Task UpdateAsync(CarCategoryDto obj) => await _repository.UpdateAsync(_mapper.Map<CarCategoryDto, CarCategory>(obj));

        public async Task DeleteAsync(Guid id)
        {
            var carCategory = await _repository.GetAsync(id);
            if (carCategory != default)
                await _repository.DeleteAsync(carCategory);
        }
        public async Task<List<CarCategoryDto>> Search(CarCategorySearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Name) && !search.Fee.HasValue)
                return _mapper.Map<List<CarCategory>, List<CarCategoryDto>>(await _repository.GetAllAsync());
            return _mapper.Map<List<CarCategory>, List<CarCategoryDto>>(await _repository.Search(search));
        }
    }
}
