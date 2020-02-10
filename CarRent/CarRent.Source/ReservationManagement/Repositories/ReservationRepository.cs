using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Source.ReservationManagement.Domain;
using CarRent.Source.ReservationManagement.Repositories.Interfaces;
using CarRent.Source.ReservationManagement.SearchHelper;

namespace CarRent.Source.ReservationManagement.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        public Task<List<Reservation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Reservation obj)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Reservation obj)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Reservation obj)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reservation>> Search(ReservationSearch search)
        {
            throw new NotImplementedException();
        }
    }
}
