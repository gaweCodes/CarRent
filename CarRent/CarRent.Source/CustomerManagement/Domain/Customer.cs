﻿using System;
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
        [MinLength(10), MaxLength(12)]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
