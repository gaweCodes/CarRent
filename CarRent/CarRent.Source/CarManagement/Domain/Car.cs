using System;

namespace CarRent.Source.CarManagement.Domain
{
    public class Car
    {
        public Guid Id { get; set; }
        public int CarNumber { get; set; }
        public Guid CarModelid { get; set; }
    }
}
