using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Source.CustomerManagement.Domain
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
