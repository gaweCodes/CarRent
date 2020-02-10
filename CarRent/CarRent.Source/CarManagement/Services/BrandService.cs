using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using CarRent.Source.CarManagement.Services.Interfaces;

namespace CarRent.Source.CarManagement.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IMapper _mapper;
        public BrandService(IBrandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brandList = await _repository.GetAllAsync();
            return _mapper.Map<List<Brand>, List<BrandDto>>(brandList);
        }
        public async Task<BrandDto> GetByIdAsync(Guid id) => _mapper.Map<Brand, BrandDto>(await _repository.GetAsync(id));
        public async Task AddAsync(BrandDto obj) => await _repository.AddAsync(_mapper.Map<BrandDto, Brand>(obj));
        public async Task UpdateAsync(BrandDto obj) => await _repository.UpdateAsync(_mapper.Map<BrandDto, Brand>(obj));

        public async Task DeleteAsync(Guid id)
        {
            var brand = await _repository.GetAsync(id);
            if (brand != default)
                await _repository.DeleteAsync(brand);
        }
        public async Task<List<BrandDto>> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _mapper.Map<List<Brand>, List<BrandDto>>(await _repository.GetAllAsync());
            return _mapper.Map<List<Brand>, List<BrandDto>>(await _repository.Search(name));
        }
    }
}
