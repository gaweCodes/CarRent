using System;

namespace CarRent.Source.ReservationManagement.SearchHelper
{
    public class ReservationSearch
    {
        public Guid? CarId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? DurationInDays { get; set; }
        public decimal? TotalCost { get; set; }
        public string State { get; set; }
    }
}
