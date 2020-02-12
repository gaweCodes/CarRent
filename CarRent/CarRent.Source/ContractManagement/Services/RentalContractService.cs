using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.ContractManagement.Domain;
using CarRent.Source.ContractManagement.Dtos;
using CarRent.Source.ContractManagement.Repositories.Interfaces;
using CarRent.Source.ContractManagement.Services.Interfaces;

namespace CarRent.Source.ContractManagement.Services
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractRepository _repository;
        private readonly IMapper _mapper;
        public RentalContractService(IRentalContractRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task CreateRentalContract(RentalContractDto obj) => await _repository.AddAsync(_mapper.Map<RentalContractDto, RentalContract>(obj));
        public async Task<List<RentalContractDto>> GetAll() => _mapper.Map<List<RentalContract>, List<RentalContractDto>>(await _repository.GetAllAsync());
        public async Task<RentalContractDto> Get(Guid id) => _mapper.Map<RentalContract, RentalContractDto>(await _repository.GetAsync(id));
    }
}
