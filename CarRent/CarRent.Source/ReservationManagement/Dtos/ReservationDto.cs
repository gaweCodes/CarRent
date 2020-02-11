using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.ReservationManagement.Dtos
{
    public class ReservationDto
    {
        [NotEmpty]
        public Guid Id { get; set; }
        [NotEmpty]
        public Guid CarId { get; set; }
        [NotEmpty]
        public Guid CustomerId { get; set; }
        [Range(1, int.MaxValue)]
        public int DurationInDays { get; set; }
        public decimal TotalCost { get; set; }
        public string State { get; set; }
    }
}
