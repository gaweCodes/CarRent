using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Source.CarManagement.Domain
{
    public abstract class VehicleClass
    {
        [Key]
        private Guid Id;
        private string Name;
        private string Type;
        private decimal DailyFee;
    }
}
