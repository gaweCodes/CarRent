using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.Services.Interfaces;
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
        public async Task<ActionResult<List<BrandDto>>> Get()
        {
            try
            {
                return await _brandService.GetAllAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest("Id must be a valid Guid");
                return await _brandService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BrandDto brandDto)
        {
            try
            {
                if (brandDto == null)  return BadRequest($"{nameof(brandDto)} can not not be null!");
                await _brandService.AddAsync(brandDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] BrandDto brandDto)
        {
            try
            {
                await _brandService.UpdateAsync(brandDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _brandService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<BrandDto>>> Search([FromQuery] string brandName)
        {
            try
            {
                return await _brandService.Search(brandName);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}