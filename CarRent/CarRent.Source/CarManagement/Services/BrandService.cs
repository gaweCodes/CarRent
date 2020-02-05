using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.Common;

namespace CarRent.Source.CarManagement.Services
{
    public class BrandService : IBrandService
    {
        private IRepository<Brand> _brandRepository;
        public BrandService(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.GetAllAsync();
        }
    }
}
