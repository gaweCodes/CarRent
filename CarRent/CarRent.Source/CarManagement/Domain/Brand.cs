﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Source.CarManagement.Domain
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
