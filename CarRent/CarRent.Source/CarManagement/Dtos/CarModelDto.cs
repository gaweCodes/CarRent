using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Dtos
{
    public class CarModelDto
    {
        [NotEmpty]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [NotEmpty]
        public Guid BrandId { get; set; }
        [NotEmpty]
        public Guid CategoryId { get; set; }
    }
}
