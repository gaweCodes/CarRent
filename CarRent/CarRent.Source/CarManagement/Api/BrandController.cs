using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Brand>>> Get()
        {
            try
            {
                return await _brandService.GetAllBrandsAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetById(Guid id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(id);
                if (brand == null)
                    return NotFound(null);
                return brand;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult<Brand>> Post([FromBody] Brand brand)
        {
            try
            {
                if (brand == null)
                    return BadRequest($"{nameof(brand)} can not not be null!");
                return await _brandService.AddBrandAsync(brand);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBrand([FromBody] Brand brand)
        {
            try
            {
                if (brand == null) return BadRequest($"{nameof(brand)} can not not be null!");
                if (await _brandService.CheckIfBrandExistAsync(brand.Id) == false) return NotFound(null);
                await _brandService.UpdateBrandAsync(brand);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(Guid id)
        {
            try
            {
                if (await _brandService.CheckIfBrandExistAsync(id) == false) return NotFound(null);
                await _brandService.DeleteBrandAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}