using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Source.CarManagement.Domain
{
    public class Car
    {
        [Key]
        private Guid Id;
        private int CarNumber;
        private string Brand;
        private string Type;
        private VehicleClass VehicleClass;
    }
}
