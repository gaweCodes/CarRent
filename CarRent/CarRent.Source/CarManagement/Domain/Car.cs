using System;

namespace CarRent.Source.CarManagement.Domain
{
    public class Car
    {
        public Guid Id { get; }
        public int CarNumber { get; }
        public Brand Brand { get; }
        public Type Type { get; }
        public CarClass VehicleClass { get; }
    }
}
