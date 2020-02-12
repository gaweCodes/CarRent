using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.ContractManagement.Domain
{
    public class RentalContract
    {
        [Key]
        public Guid Id { get; set; }
        [NotEmpty]
        public Guid ReservationId { get; set; }
        [NotEmpty]
        public Guid CarId { get; set; }
        [NotEmpty]
        public Guid CustomerId { get; set; }
        [Range(1, int.MaxValue)]
        public int DurationInDays { get; set; }
        [Range(typeof(decimal), "1", "79228162514264337593543950335")]
        public decimal TotalCost { get; set; }
    }
}
