using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.ReservationManagement.Domain;
using CarRent.Source.ReservationManagement.SearchHelper;

namespace CarRent.Source.ReservationManagement.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation> GetAsync(Guid id);
        Task AddAsync(Reservation obj);
        Task DeleteAsync(Reservation obj);
        Task UpdateAsync(Reservation obj);
        Task<List<Reservation>> Search(ReservationSearch search);
    }
}
