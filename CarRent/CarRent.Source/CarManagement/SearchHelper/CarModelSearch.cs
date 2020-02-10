using System;

namespace CarRent.Source.CarManagement.SearchHelper
{
    public class CarModelSearch
    {
        public string Name { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
