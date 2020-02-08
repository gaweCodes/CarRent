using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Dtos
{
    public class CarDto
    {
        [NotEmpty]
        public Guid Id { get; set; }
        [Required]
        public string CarNumber { get; set; }
        [NotEmpty]
        public Guid CarModelid { get; set; }
    }
}
