using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;

namespace CarRent.Source.CarManagement.Services
{
    public interface IBrandService
    {
        public Task<List<Brand>> GetAllBrandsAsync();
    }
}
