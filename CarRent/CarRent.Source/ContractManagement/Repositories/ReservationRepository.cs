using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.ContractManagement.Repositories.Interfaces;
using CarRent.Source.Database;
using CarRent.Source.ContractManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.ContractManagement.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RentalContractRepository : IRentalContractRepository
    {
        private readonly CarRentDbContext _dbContext;
        public RentalContractRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RentalContract>> GetAllAsync() =>
            await _dbContext.RentalContracts.AsQueryable().OrderBy(r => r.TotalCost).ToListAsync();

        public async Task<RentalContract> GetAsync(Guid id) =>
            await _dbContext.RentalContracts.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(RentalContract obj)
        {
            await _dbContext.RentalContracts.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(RentalContract obj)
        {
            _dbContext.RentalContracts.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }
    }
}
