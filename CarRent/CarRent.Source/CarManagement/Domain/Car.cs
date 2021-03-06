﻿using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Domain
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string CarNumber { get; set; }
        [NotEmpty]
        public Guid CarModelid { get; set; }
    }
}
