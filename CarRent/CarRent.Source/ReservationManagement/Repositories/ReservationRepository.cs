using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.Database;
using CarRent.Source.ReservationManagement.Domain;
using CarRent.Source.ReservationManagement.Repositories.Interfaces;
using CarRent.Source.ReservationManagement.SearchHelper;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.ReservationManagement.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CarRentDbContext _dbContext;
        public ReservationRepository(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Reservation>> GetAllAsync() =>
            await _dbContext.Reservations.AsQueryable().OrderBy(r => r.State).ToListAsync();

        public async Task<Reservation> GetAsync(Guid id) =>
            await _dbContext.Reservations.AsNoTracking().AsQueryable().SingleOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(Reservation obj)
        {
            await _dbContext.Reservations.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation obj)
        {
            _dbContext.Reservations.Remove(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation obj)
        {
            _dbContext.Reservations.Update(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Reservation>> Search(ReservationSearch search)
        {
            var query = _dbContext.Reservations.AsNoTracking().AsQueryable();
            if (search.State.HasValue)
                query = query.Where(r => r.State == search.State);
            if (search.CarId.HasValue)
                query = query.Where(r => r.CarId == search.CarId.Value);
            if (search.DurationInDays.HasValue)
                query = query.Where(r => r.DurationInDays == search.DurationInDays.Value);
            if (search.CustomerId.HasValue)
                query = query.Where(r =>r.CustomerId == search.CustomerId.Value);
            if (search.TotalCost.HasValue)
                query = query.Where(r => r.TotalCost == search.TotalCost.Value);

            return await query.OrderBy(r => r.State).ToListAsync();
        }
    }
}
