using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Domain
{
    public class CarModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [NotEmpty]
        public Guid BrandId { get; set; }
        [NotEmpty]
        public Guid CategoryId { get; set; }
    }
}
