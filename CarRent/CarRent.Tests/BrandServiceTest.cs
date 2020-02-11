namespace CarRent.Tests
{
    using CarRent.Source.CarManagement.Domain;
    using CarRent.Source.CarManagement.Dtos;
    using CarRent.Source.CarManagement.Services;
    using CarRent.Source.CarManagement.Services.Interfaces;

    using NUnit.Framework;

    public class Tests
    {
        [Test]
        public void BrandService_GetAllBrands_ListOfBrands()
        {
            // Arange
            CreateMap<BrandDto, Brand>;
            var brandservice = new BrandService();
            // Act

            // Assert
            Assert.Pass();
        }
    }
}