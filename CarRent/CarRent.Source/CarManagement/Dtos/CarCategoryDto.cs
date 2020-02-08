using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Dtos
{
    public class CarCategoryDto
    {
        [NotEmpty]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(typeof(decimal), "1", "79228162514264337593543950335")]
        public decimal DailyFee { get; set; }
    }
}
