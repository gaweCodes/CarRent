using System;

namespace CarRent.Source.CarManagement.Domain
{
    public class CarClass
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Type { get; }
        public decimal DailyFee { get; }
    }
}
