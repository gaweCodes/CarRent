using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.ContractManagement.Domain;

namespace CarRent.Source.ContractManagement.Repositories.Interfaces
{
    public interface IRentalContractRepository
    {
        Task<List<RentalContract>> GetAllAsync();
        Task<RentalContract> GetAsync(Guid id);
        Task AddAsync(RentalContract obj);
        Task DeleteAsync(RentalContract obj);
    }
}
