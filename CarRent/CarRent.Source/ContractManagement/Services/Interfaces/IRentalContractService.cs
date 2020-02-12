using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.ContractManagement.Dtos;

namespace CarRent.Source.ContractManagement.Services.Interfaces
{
    public interface IRentalContractService
    {
        public Task CreateRentalContract(RentalContractDto obj);
        public Task<List<RentalContractDto>> GetAll();
        public Task<RentalContractDto> Get(Guid id);
    }
}
