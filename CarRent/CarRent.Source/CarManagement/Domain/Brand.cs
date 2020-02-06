using System;

namespace CarRent.Source.CarManagement.Domain
{
    public class Brand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; }
    }
}
