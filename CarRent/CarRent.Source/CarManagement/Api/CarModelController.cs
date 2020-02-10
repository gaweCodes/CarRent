using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.SearchHelper;
using CarRent.Source.CarManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : Controller
    {
        private readonly ICarModelService _carModelService;
        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CarModelDto>>> Get()
        {
            try
            {
                return await _carModelService.GetAllAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CarModelDto>> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest("Id must be a valid Guid");
                return await _carModelService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarModelDto carModelDto)
        {
            try
            {
                if (carModelDto == null) return BadRequest($"{nameof(carModelDto)} can not not be null!");
                await _carModelService.AddAsync(carModelDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] CarModelDto carModelDto)
        {
            try
            {
                await _carModelService.UpdateAsync(carModelDto);
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
                await _carModelService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<CarModelDto>>> Search([FromQuery] string name, [FromQuery] Guid? brandId, [FromQuery] Guid? categoryId)
        {
            try
            {
                return await _carModelService.Search(new CarModelSearch { Name = name, BrandId = brandId, CategoryId = categoryId});
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}