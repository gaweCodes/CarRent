using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.ReservationManagement.Dtos;
using CarRent.Source.ReservationManagement.SearchHelper;

namespace CarRent.Source.ReservationManagement.Services.Interfaces
{
    public interface IReservationService
    {
        public Task<List<ReservationDto>> GetAllAsync();
        public Task<ReservationDto> GetByIdAsync(Guid id);
        public Task AddAsync(ReservationDto obj);
        public Task UpdateAsync(ReservationDto obj);
        public Task DeleteAsync(Guid id);
        public Task<List<ReservationDto>> Search(ReservationSearch search);
    }
}
