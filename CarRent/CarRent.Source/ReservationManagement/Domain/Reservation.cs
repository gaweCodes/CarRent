using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;
using CarRent.Source.ReservationManagement.Dtos;

namespace CarRent.Source.ReservationManagement.Domain
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        [NotEmpty]
        public Guid CarId { get; set; }
        [NotEmpty]
        public Guid CustomerId { get; set; }
        [Range(1, int.MaxValue)]
        public int DurationInDays { get; set; }
        [Range(typeof(decimal), "1", "79228162514264337593543950335")]
        public decimal DailyFee { get; set; }
        [Range(typeof(decimal), "1", "79228162514264337593543950335")]
        public decimal TotalCost { get; set; }
        [Required]
        public ReservationState State { get; set; }
        public void SetActive() => State = ReservationState.Active;
        public void SetClosed() => State = ReservationState.Closed;
        public void CalculateTotal() => TotalCost = DailyFee * DurationInDays;
        public static ReservationDto ToDto(Reservation source) => new ReservationDto
            {
                CarId = source.CarId,
                CustomerId = source.CustomerId,
                DurationInDays = source.DurationInDays,
                Id = source.Id,
                State = source.State.ToString(),
                TotalCost = source.TotalCost
            };

        public static Reservation FromDto(ReservationDto source) => new Reservation
        {
            Id = source.Id, 
            CarId = source.CarId, 
            CustomerId = source.CustomerId,
            DurationInDays = source.DurationInDays, 
            TotalCost = source.TotalCost,
        };
    }
}
