﻿using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Dtos
{
    public class BrandDto
    {
        [NotEmpty]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
