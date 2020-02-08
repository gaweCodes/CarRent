using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Source.CarManagement.Domain
{
    public class CarCategory
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(typeof(decimal), "1", "79228162514264337593543950335")]
        public decimal DailyFee { get; set; }
    }
}
