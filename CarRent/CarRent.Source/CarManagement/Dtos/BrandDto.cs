using System;

namespace CarRent.Source.CarManagement.Dtos
{
    public class BrandDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
    }
}
